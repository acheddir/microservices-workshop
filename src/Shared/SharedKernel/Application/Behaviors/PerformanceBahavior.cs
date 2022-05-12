﻿using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Application.Common.Services;

namespace SharedKernel.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehavior(
        Stopwatch timer,
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        ICurrentUserService currentUserService)
    {
        _timer = timer;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            var userName = _currentUserService.UserName;
            
            _logger.LogWarning("--> Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);
        }
        
        return response;
    }
}