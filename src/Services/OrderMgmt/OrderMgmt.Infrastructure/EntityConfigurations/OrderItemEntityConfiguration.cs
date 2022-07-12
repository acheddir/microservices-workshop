namespace OrderMgmt.Infrastructure.EntityConfigurations;

internal class OrderItemEntityConfiguration
    : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> orderItemConfiguration)
    {
        orderItemConfiguration.ToTable("order_items", OrderMgmtContext.DefaultSchema);

        orderItemConfiguration.HasKey(o => o.Id);
        
        orderItemConfiguration.Ignore(b => b.DomainEvents);

        orderItemConfiguration.Property<Guid>("OrderId")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_discount")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("discount")
            .IsRequired();

        orderItemConfiguration.Property<Guid>("ProductId")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_productName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("product_name")
            .IsRequired();

        orderItemConfiguration
            .Property<decimal>("_unitPrice")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("unit_price")
            .IsRequired();

        orderItemConfiguration
            .Property<int>("_units")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("units")
            .IsRequired();

        orderItemConfiguration
            .Property<string>("_pictureUrl")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("picture_url")
            .IsRequired(false);
    }
}