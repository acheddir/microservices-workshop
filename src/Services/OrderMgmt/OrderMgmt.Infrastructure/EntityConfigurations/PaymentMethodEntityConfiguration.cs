namespace OrderMgmt.Infrastructure.EntityConfigurations;

public class PaymentMethodEntityConfiguration
    : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("payment_methods", OrderMgmtContext.DefaultSchema);

        builder.HasKey(b => b.Id);
        builder.Ignore(b => b.DomainEvents);

        builder.Property<Guid>("CustomerId")
            .IsRequired();

        builder
            .Property<string>("_cardHolderName")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_holder_name")
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property<string>("_alias")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("alias")
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property<string>("_cardNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_number")
            .HasMaxLength(25)
            .IsRequired();

        builder
            .Property<DateTime>("_expiration")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("expiration")
            .HasMaxLength(25)
            .IsRequired();

        builder
            .Property<int>("_cardTypeId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("card_type_id")
            .IsRequired();

        builder.HasOne(p => p.CardType)
            .WithMany()
            .HasForeignKey("_cardTypeId");
    }
}
