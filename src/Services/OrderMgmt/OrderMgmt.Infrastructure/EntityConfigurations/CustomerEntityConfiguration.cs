namespace OrderMgmt.Infrastructure.EntityConfigurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers", OrderMgmtContext.DefaultSchema);

        builder.HasKey(b => b.Id);
        builder.Ignore(b => b.DomainEvents);

        builder.Property(b => b.IG)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasIndex("IG")
            .IsUnique(true);

        builder.Property(b => b.Name);

        builder.HasMany(b => b.PaymentMethods)
            .WithOne()
            .HasForeignKey("CustomerId")
            .OnDelete(DeleteBehavior.Cascade);

        var navigation = builder.Metadata.FindNavigation(nameof(Customer.PaymentMethods));

        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}