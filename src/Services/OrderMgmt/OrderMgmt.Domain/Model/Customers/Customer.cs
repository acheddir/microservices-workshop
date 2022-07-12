using OrderMgmt.Domain.DomainEvents;

namespace OrderMgmt.Domain.Model.Customers;

public class Customer : BaseEntity, IAggregateRoot
{
    public string IG { get; private set; }
    public string Name { get; private set; }
    private readonly List<PaymentMethod> _paymentMethods;
    
    public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

    public Customer()
    {
        _paymentMethods = new List<PaymentMethod>();
    }

    public Customer(string ig, string name) : this()
    {
        IG = !string.IsNullOrWhiteSpace(ig) ? ig : throw new ArgumentNullException(nameof(ig));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
    }

    public PaymentMethod VerifyOrAddPaymentMethod(
        int cardTypeId, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateTime expiration, int orderId)
    {
        var existingPayment = _paymentMethods
            .SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

        if (existingPayment != null)
        {
            AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));
            return existingPayment;
        }
        
        var payment = new PaymentMethod(cardTypeId, alias, cardNumber, securityNumber, cardHolderName, expiration);
        _paymentMethods.Add(payment);

        AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));
        return payment;
    }
}