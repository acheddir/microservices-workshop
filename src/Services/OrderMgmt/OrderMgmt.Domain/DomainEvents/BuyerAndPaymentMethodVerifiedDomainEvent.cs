namespace OrderMgmt.Domain.DomainEvents;

public class CustomerAndPaymentMethodVerifiedDomainEvent : INotification
{
    public Customer Customer { get; private set; }
    public PaymentMethod Payment { get; private set; }
    public int OrderId { get; private set; }

    public CustomerAndPaymentMethodVerifiedDomainEvent(Customer customer, PaymentMethod payment, int orderId)
    {
        Customer = customer;
        Payment = payment;
        OrderId = orderId;
    }
}