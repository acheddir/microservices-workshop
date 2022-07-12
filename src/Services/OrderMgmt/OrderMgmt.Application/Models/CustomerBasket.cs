namespace OrderMgmt.Application.Models;

public class CustomerBasket
{
    public string CustomerId { get; set; }
    public List<BasketItem> Items { get; set; }
    
    public CustomerBasket(string customerId, List<BasketItem> items)
    {
        CustomerId = customerId;
        Items = items;
    }
}
