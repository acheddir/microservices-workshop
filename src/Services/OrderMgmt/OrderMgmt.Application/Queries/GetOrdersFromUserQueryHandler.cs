namespace OrderMgmt.Application.Queries;

public class GetOrdersFromUserQueryHandler : IRequestHandler<GetOrdersFromUserQuery, IEnumerable<OrderSummary>>
{
    private const string ConnectionStringName = "OrderMgmtDb";
    
    private readonly IConfiguration _configuration;

    public GetOrdersFromUserQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<OrderSummary>> Handle(GetOrdersFromUserQuery request, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString(ConnectionStringName));
        await connection.OpenAsync(cancellationToken);

        return await connection.QueryAsync<OrderSummary>(
            @"select o.id as ordernumber, o.order_date as date, os.name as status, sum(oi.units*oi.unit_price) as total
                from order_mgmt.orders o
                left join order_mgmt.order_items oi on o.id = oi.order_id
                left join order_mgmt.order_status os on o.order_status_id = os.id
                left join order_mgmt.customers ob on o.customer_id = ob.id
                where ob.ig = @user_id
                group by o.id, o.order_date, os.name
                order by o.id", new { user_id = request.UserId });
    }
}