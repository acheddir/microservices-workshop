using SharedKernel.Domain.Exceptions;

namespace OrderMgmt.Application.Queries;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
{
    private const string ConnectionStringName = "OrderMgmtDb";
    
    private readonly IConfiguration _configuration;

    public GetOrderQueryHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        await using var connection = new NpgsqlConnection(_configuration.GetConnectionString(ConnectionStringName));
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<dynamic>(
            @"select o.id as ordernumber, o.order_date as date, o.description as description,
                o.address_city as city, o.address_country as country, o.address_state as state,
                o.address_street as street, o.address_zip_code as zipcode,
                os.name as status, 
                oi.product_name as productname, oi.units as units, oi.unit_price as unitprice, oi.picture_url as pictureurl
                FROM order_mgmt.orders o
                LEFT JOIN order_mgmt.order_items oi ON o.id = oi.order_id 
                LEFT JOIN order_mgmt.order_status os on o.order_status_id = os.id
                WHERE o.id=@id"
            , new { id = request.OrderId }
        );

        if (result.AsList().Count == 0)
            throw new NotFoundException();

        return MapOrderItems(result);
    }
    
    private Order MapOrderItems(dynamic result)
    {
        var order = new Order
        {
            ordernumber = result[0].ordernumber,
            date = result[0].date,
            status = result[0].status,
            description = result[0].description,
            street = result[0].street,
            city = result[0].city,
            zipcode = result[0].zipcode,
            country = result[0].country,
            orderitems = new List<OrderItem>(),
            total = 0
        };

        foreach (var item in result)
        {
            var orderItem = new OrderItem
            {
                productname = item.productname,
                units = item.units,
                unitprice = (double)item.unitprice,
                pictureurl = item.pictureurl
            };

            order.total += item.units * item.unitprice;
            order.orderitems.Add(orderItem);
        }

        return order;
    }
}