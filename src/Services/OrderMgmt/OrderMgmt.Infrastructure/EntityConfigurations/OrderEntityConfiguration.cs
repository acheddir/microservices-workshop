namespace OrderMgmt.Infrastructure.EntityConfigurations;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders", OrderMgmtContext.DefaultSchema);

        builder.HasKey(o => o.Id);
        builder.Ignore(b => b.DomainEvents);
        
        //Address value object persisted as owned entity type supported since EF Core 2.0
        builder
            .OwnsOne(o => o.Address, a =>
            {
                // Explicit configuration of the shadow key property in the owned type 
                // as a workaround for a documented issue in EF Core 5: https://github.com/dotnet/efcore/issues/20740
                a.Property<Guid>("OrderId");
                a.WithOwner();
            });

        builder
            .Property<Guid?>("_customerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("customer_id")
            .IsRequired(false);

        builder
            .Property<DateTime>("_orderDate")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("order_date")
            .IsRequired();

        builder
            .Property<int>("_orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("order_status_id")
            .IsRequired();

        builder
            .Property<Guid?>("_paymentMethodId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("payment_method_id")
            .IsRequired(false);

        builder.Property<string>("Description").IsRequired(false);

        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));

        // DDD Patterns comment:
        //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne<Customer>()
            .WithMany()
            .IsRequired(false)
            .HasForeignKey("_customerId");

        builder.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("_orderStatusId");
    }
}