namespace OrderMgmt.Domain.Model.Customers;

public class PaymentMethod : BaseEntity
{
    private string _alias;
    private readonly string _cardNumber;
    private string _securityNumber;
    private string _cardHolderName;
    private readonly DateTime _expiration;

    private readonly int _cardTypeId;
    public CardType CardType { get; private set; }

    protected PaymentMethod() { }

    public PaymentMethod(int cardTypeId,
        string alias,
        string cardNumber,
        string securityNumber,
        string cardHolderName,
        DateTime expiration)
    {
        _cardNumber = !string.IsNullOrWhiteSpace(cardNumber)
            ? cardNumber
            : throw new OrderMgmtException(nameof(cardNumber));

        _securityNumber = !string.IsNullOrWhiteSpace(securityNumber)
            ? securityNumber
            : throw new OrderMgmtException(nameof(securityNumber));

        _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName)
            ? cardHolderName
            : throw new OrderMgmtException(nameof(cardHolderName));

        if (expiration < DateTime.UtcNow)
        {
            throw new OrderMgmtException(nameof(expiration));
        }

        _alias = alias;
        _expiration = expiration;
        _cardTypeId = cardTypeId;
    }

    public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
    {
        return _cardTypeId == cardTypeId
               && _cardNumber == cardNumber
               && _expiration == expiration;
    }
}