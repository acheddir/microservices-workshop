namespace OrderMgmt.Infrastructure;

public class OrderMgmtContext : BaseDbContext
{
    public const string DefaultSchema = "order_mgmt";

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();
    public DbSet<PaymentMethod> Payments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CardType> CardTypes { get; set; }
 
    public OrderMgmtContext(DbContextOptions<OrderMgmtContext> options, ICurrentUserService currentUserService, IMediator mediator)
        : base(options, currentUserService, mediator)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}