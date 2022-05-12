using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderMgmt.Domain.Model.Orders;
using SharedKernel.Infrastructure.Common;

namespace OrderMgmt.Infrastructure;

public class OrderMgmtContext : BaseDbContext
{
    public const string DefaultSchema = "order_mgmt";

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();
 
    public OrderMgmtContext(DbContextOptions<OrderMgmtContext> options, IMediator mediator)
        : base(options, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}