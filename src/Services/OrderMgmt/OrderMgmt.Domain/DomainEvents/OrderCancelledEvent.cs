using MediatR;
using OrderMgmt.Domain.Model.Orders;

namespace OrderMgmt.Domain.DomainEvents;

public class OrderCancelledEvent : INotification
{
    public Order Order { get; }

    public OrderCancelledEvent(Order order)
    {
        Order = order;
    }
}