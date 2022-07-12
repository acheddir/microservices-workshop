namespace OrderMgmt.Application.Commands;

public class CreateOrderDraftCommand : IRequest<OrderDraftDto>
{
    public string CustomerId { get; private set; }
    public IEnumerable<BasketItem> Items { get; private set; }
    
    public CreateOrderDraftCommand(string customerId, IEnumerable<BasketItem> items)
    {
        CustomerId = customerId;
        Items = items;
    }
}
