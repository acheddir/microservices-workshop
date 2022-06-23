using MassTransit;
using SharedKernel.Application.Common.Services;
using SharedKernel.EventBus.Events;

namespace OrderMgmt.Application.Services;

public class OrderMgmtIntegrationEventService : IIntegrationEventService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderMgmtIntegrationEventService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        throw new NotImplementedException();
    }

    public Task AddAndSaveEventAsync(IntegrationEvent evt)
    {
        throw new NotImplementedException();
    }
}