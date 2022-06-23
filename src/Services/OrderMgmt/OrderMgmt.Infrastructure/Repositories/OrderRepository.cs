using Microsoft.EntityFrameworkCore;
using OrderMgmt.Domain.Model.Orders;
using SharedKernel.Domain.Common;

namespace OrderMgmt.Infrastructure.Repositories;

public class OrderRepository
    : IOrderRepository
{
    private readonly OrderMgmtContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public OrderRepository(OrderMgmtContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public Order Add(Order order)
    {
        return _context.Orders.Add(order).Entity;
    }

    public void Update(Order order)
    {
        _context.Entry(order).State = EntityState.Modified;
    }

    public async Task<Order?> GetAsync(Guid orderId)
    {
        var order =
            (await _context.Orders.Include(x => x.Address).FirstOrDefaultAsync(o => o.Id == orderId))
            ?? (_context.Orders.Local.FirstOrDefault(o => o.Id == orderId));

        if (order == null) return order;
        
        await _context.Entry(order)
            .Collection(i => i.OrderItems).LoadAsync();
        await _context.Entry(order)
            .Reference(i => i.OrderStatus).LoadAsync();

        return order;
    }
}