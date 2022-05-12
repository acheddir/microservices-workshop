using MediatR;
using OrderMgmt.Domain.Model.Orders;

namespace OrderMgmt.Domain.DomainEvents;

public class OrderStatusChangedToPaidEvent : INotification
{
    public Guid OrderId { get; }
    public IEnumerable<OrderItem> OrderItems { get; }

    public OrderStatusChangedToPaidEvent(
        Guid orderId,
        IEnumerable<OrderItem> orderItems)
    {
        OrderId = orderId;
        OrderItems = orderItems;
    }
}