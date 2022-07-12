namespace OrderMgmt.Application.Queries;

public class GetOrderQuery : IQuery<Order>
{
    [DataMember]
    public Guid OrderId { get; set; }

    public GetOrderQuery(Guid orderId)
    {
        OrderId = orderId;
    }
}