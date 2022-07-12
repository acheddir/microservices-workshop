namespace OrderMgmt.Infrastructure.EntityConfigurations;

internal class OrderStatusEntityConfiguration
    : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> orderStatusConfiguration)
    {
        orderStatusConfiguration.ToTable("order_status", OrderMgmtContext.DefaultSchema);

        orderStatusConfiguration.HasKey(o => o.Id);

        orderStatusConfiguration.Property(o => o.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        orderStatusConfiguration.Property(o => o.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}