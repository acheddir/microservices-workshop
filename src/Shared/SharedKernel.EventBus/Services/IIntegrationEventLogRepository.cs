﻿using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel.EventBus.Events;

namespace SharedKernel.EventBus.Services;

public interface IIntegrationEventLogRepository
{
    Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
    Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
    Task MarkEventAsPublishedAsync(Guid eventId);
    Task MarkEventAsInProgressAsync(Guid eventId);
    Task MarkEventAsFailedAsync(Guid eventId);
}