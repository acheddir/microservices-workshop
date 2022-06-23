using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using SharedKernel.Application.Common;
using SharedKernel.Application.Common.Services;
using SharedKernel.Extensions;
using SharedKernel.Infrastructure.Common;

namespace SharedKernel.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly ILogger _logger;
    private readonly BaseDbContext _context;
    private readonly IIntegrationEventService _integrationEventService;
    
    public TransactionBehavior(
        Logger<TransactionBehavior<TRequest, TResponse>> logger,
        BaseDbContext context,
        IIntegrationEventService integrationEventService)
    {
        _logger = logger;
        _context = context;
        _integrationEventService = integrationEventService;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (_context.HasActiveTransaction)
                return await next();
            
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await _context.BeginTransactionAsync();
                using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                {
                    _logger.LogInformation("--> Begin Transaction {TransactionId} for {CommandName} ({@Command})",
                        transaction.TransactionId,
                        typeName,
                        request);

                    response = await next();
                    
                    _logger.LogInformation("--> Committed Transaction {TransactionId} for {CommandName} ({@Command})",
                        transaction.TransactionId,
                        typeName,
                        request);

                    transactionId = transaction.TransactionId;
                }

                await _integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
            });

            return response!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "--> ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}