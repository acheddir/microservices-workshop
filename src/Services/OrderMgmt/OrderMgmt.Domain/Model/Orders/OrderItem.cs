namespace OrderMgmt.Domain.Model.Orders;

public class OrderItem : BaseEntity
{
    private readonly string? _productName;
    private readonly string? _pictureUrl;
    private readonly decimal _unitPrice;
    private decimal _discount;
    private int _units;

    public Guid ProductId { get; private set; }

    public OrderItem() { }
    
    public OrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        if (units <= 0)
        {
            throw new OrderMgmtException("Invalid number of units");
        }

        if ((unitPrice * units) < discount)
        {
            throw new OrderMgmtException("The total of order item is lower than the applied discount");
        }

        ProductId = productId;

        _productName = productName;
        _unitPrice = unitPrice;
        _discount = discount;
        _units = units;
        _pictureUrl = pictureUrl;
    }

    public string? GetPictureUri() => _pictureUrl;

    public decimal? GetCurrentDiscount()
    {
        return _discount;
    }

    public int GetUnits()
    {
        return _units;
    }

    public decimal? GetUnitPrice()
    {
        return _unitPrice;
    }

    public string? GetProductName() => _productName;

    public void SetNewDiscount(decimal discount)
    {
        if (discount < 0)
        {
            throw new OrderMgmtException("Discount is not valid");
        }
        if ((_unitPrice * _units) < discount)
        {
            throw new OrderMgmtException("The total of order item is lower than the applied discount");
        }

        _discount = discount;
    }

    public void AddUnits(int units)
    {
        if (units + _units <= 0)
        {
            throw new OrderMgmtException("Invalid units");
        }

        _units += units;
    }
}