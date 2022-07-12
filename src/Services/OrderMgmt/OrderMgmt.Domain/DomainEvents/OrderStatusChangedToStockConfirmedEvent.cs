namespace OrderMgmt.Domain.DomainEvents;

public class OrderStatusChangedToStockConfirmedEvent : INotification
{
    public Guid OrderId { get; }

    public OrderStatusChangedToStockConfirmedEvent(Guid orderId) => OrderId = orderId;
}