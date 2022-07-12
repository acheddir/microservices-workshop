using System.Data.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderMgmt.Infrastructure;
using SharedKernel.Application.Common.Services;
using SharedKernel.EventBus.Events;
using SharedKernel.EventBus.Services;

namespace OrderMgmt.Application.Services;

public class OrderMgmtIntegrationEventService : IIntegrationEventService
{
    private readonly Func<DbConnection, IIntegrationEventLogRepository> _integrationEventLogRepositoryFactory;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly OrderMgmtContext _context;
    private readonly IIntegrationEventLogRepository _eventLogRepository;
    private readonly ILogger _logger;

    public OrderMgmtIntegrationEventService(
        Func<DbConnection, IIntegrationEventLogRepository> integrationEventLogRepositoryFactory,
        IPublishEndpoint publishEndpoint,
        OrderMgmtContext context,
        ILogger<OrderMgmtIntegrationEventService> logger)
    {
        _integrationEventLogRepositoryFactory = integrationEventLogRepositoryFactory ?? throw new ArgumentNullException(nameof(integrationEventLogRepositoryFactory));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _eventLogRepository = _integrationEventLogRepositoryFactory(_context.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingEventLogs = await _eventLogRepository.RetrieveEventLogsPendingToPublishAsync(transactionId);

        foreach (var eventLog in pendingEventLogs)
        {
            _logger.LogInformation("--> Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", eventLog.EventId, eventLog.IntegrationEvent);

            try
            {
                await _eventLogRepository.MarkEventAsInProgressAsync(eventLog.EventId);
                await _publishEndpoint.Publish(eventLog.IntegrationEvent!, eventLog.IntegrationEvent!.GetType());
                await _eventLogRepository.MarkEventAsPublishedAsync(eventLog.EventId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "--> ERROR publishing integration event: {IntegrationEventId}", eventLog.EventId);

                await _eventLogRepository.MarkEventAsFailedAsync(eventLog.EventId);
            }
        }
    }

    public async Task AddAndSaveEventAsync(IntegrationEvent @event)
    {
        _logger.LogInformation("--> Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", @event.Id, @event);

        await _eventLogRepository.SaveEventAsync(@event, _context.GetCurrentTransaction());
    }
}