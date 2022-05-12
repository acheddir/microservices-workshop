using MediatR;
using OrderMgmt.Domain.Model.Orders;

namespace OrderMgmt.Domain.DomainEvents;

public class OrderStartedEvent : INotification
{
    public OrderStartedEvent(
        Guid userId,
        string userName,
        Guid cardTypeId,
        string cardNumber,
        string cardSecurityNumber,
        string cardHolderName,
        DateTime cardExpiration,
        Order order)
    {
        UserId = userId;
        UserName = userName;
        CardTypeId = cardTypeId;
        CardNumber = cardNumber;
        CardSecurityNumber = cardSecurityNumber;
        CardHolderName = cardHolderName;
        CardExpiration = cardExpiration;
        Order = order;
    }

    public Guid UserId { get; }
    public string UserName { get; }
    public Guid CardTypeId { get; }
    public string CardNumber { get; }
    public string CardSecurityNumber { get; }
    public string CardHolderName { get; }
    public DateTime CardExpiration { get; }
    public Order Order { get; }
}