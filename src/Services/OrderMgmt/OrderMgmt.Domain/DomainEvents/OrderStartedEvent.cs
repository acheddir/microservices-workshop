using OrderMgmt.Domain.Model.Orders;

namespace OrderMgmt.Domain.DomainEvents;

public class UserEventData
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}

public class CardEventData
{
    public Guid CardTypeId { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public string CardSecurityNumber { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public DateTime CardExpiration { get; set; }
}

public class OrderStartedEvent : INotification
{
    public OrderStartedEvent(
        UserEventData userData,
        CardEventData cardData,
        Order order)
    {
        UserId = userData.UserId;
        UserName = userData.UserName;
        CardTypeId = cardData.CardTypeId;
        CardNumber = cardData.CardNumber;
        CardSecurityNumber = cardData.CardSecurityNumber;
        CardHolderName = cardData.CardHolderName;
        CardExpiration = cardData.CardExpiration;
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