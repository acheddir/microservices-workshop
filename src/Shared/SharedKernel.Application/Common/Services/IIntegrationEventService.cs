﻿using SharedKernel.EventBus.Events;

namespace SharedKernel.Application.Common.Services;

public interface IIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}
