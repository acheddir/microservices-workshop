using MediatR;
using OrderMgmt.Domain.Model.Orders;

namespace OrderMgmt.Domain.DomainEvents;

public class OrderShippedEvent : INotification
{
    public OrderShippedEvent(Order order)
    {
        Order = order;
    }

    public Order Order { get; set; }
}