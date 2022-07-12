namespace OrderMgmt.Application.Queries;

public class GetOrdersFromUserQuery : IQuery<IEnumerable<OrderSummary>>
{
    [DataMember]
    public string? UserId { get; set; }

    public GetOrdersFromUserQuery(string? userId)
    {
        UserId = userId;
    }
}