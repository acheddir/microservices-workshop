using OrderMgmt.Domain.DomainEvents;
using OrderMgmt.Domain.Exceptions;
using SharedKernel.Domain.Common;

namespace OrderMgmt.Domain.Model.Orders;

public class Order : BaseEntity, IAggregateRoot
{
    #region Fields

    private DateTime _orderDate;
    private Guid? _customerId;
    private int _orderStatusId;
    private string _description;
    
    private bool _isDraft;
    
    private readonly List<OrderItem> _orderItems;

    private Guid? _paymentMethodId;
    
    #endregion
    
    public Order()
    {
        _description = string.Empty;
        _orderItems = new List<OrderItem>();
        _isDraft = false;

        _orderStatusId = OrderStatus.Initial.Id;
        OrderStatus = OrderStatus.From(_orderStatusId);
    }
    
    public Order(
        Guid userId,
        string userName,
        Address address,
        Guid cardTypeId,
        string cardNumber,
        string cardSecurityNumber,
        string cardHolderName,
        DateTime cardExpiration, string description, OrderStatus orderStatus, Guid? customerId = null,
        Guid? paymentMethodId = null) : this()
    {
        _customerId = customerId;
        _paymentMethodId = paymentMethodId;
        _orderDate = DateTime.UtcNow;
        _orderStatusId = OrderStatus.Submitted.Id;
        
        Address = address;

        // Add the OrderStartedEvent to the domain events collection 
        // to be raised/dispatched when committing changes into the Database [ After DbContext.SaveChanges() ]
        AddDomainEvent(
            new OrderStartedEvent(
                userId,
                userName,
                cardTypeId,
                cardNumber,
                cardSecurityNumber,
                cardHolderName,
                cardExpiration,
                this));
    }

    #region Properties

    public Address? Address { get; private set; }

    public OrderStatus OrderStatus
    {
        get;
        private set;
    }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
    public void SetPaymentMethodId(Guid paymentMethodId)
    {
        _paymentMethodId = paymentMethodId;
    }
    public void SetCustomerId(Guid customerId)
    {
        _customerId = customerId;
    }

    public string? Description => _description;
    public bool IsDraft => _isDraft;
    
    public decimal? Total => _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());

    #endregion

    public static Order NewDraft()
    {
        var order = new Order
        {
            _isDraft = true
        };
        return order;
    }
    
    public void AddOrderItem(Guid productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
    {
        var existingOrderForProduct = _orderItems
            .SingleOrDefault(o => o.ProductId == productId);

        if (existingOrderForProduct != null)
        {
            if (discount > existingOrderForProduct.GetCurrentDiscount())
            {
                existingOrderForProduct.SetNewDiscount(discount);
            }

            existingOrderForProduct.AddUnits(units);
        }
        else
        {
            var orderItem = new OrderItem(productId, productName, unitPrice, discount, pictureUrl, units);
            _orderItems.Add(orderItem);
        }
    }

    public void SetAwaitingValidationStatus()
    {
        if (OrderStatus.Id != OrderStatus.Submitted.Id) return;
        
        AddDomainEvent(new OrderStatusChangedToAwaitingValidationEvent(Id, OrderItems));
        
        _orderStatusId = OrderStatus.AwaitingValidation.Id;
    }
    
    public void SetStockConfirmedStatus()
    {
        if (OrderStatus.Id != OrderStatus.AwaitingValidation.Id) return;
        
        AddDomainEvent(new OrderStatusChangedToStockConfirmedEvent(Id));

        _orderStatusId = OrderStatus.StockConfirmed.Id;
        _description = "All the items were confirmed with available stock.";
    }
    
    public void SetPaidStatus()
    {
        if (OrderStatus.Id != OrderStatus.StockConfirmed.Id) return;
        
        AddDomainEvent(new OrderStatusChangedToPaidEvent(Id, OrderItems));

        _orderStatusId = OrderStatus.Paid.Id;
        _description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\".";
    }
    
    public void SetShippedStatus()
    {
        if (OrderStatus.Id != OrderStatus.Paid.Id)
        {
            throw new OrderMgmtException(
                $"Is not possible to change the order status from {OrderStatus.Name} to {OrderStatus.Shipped.Name}.");
        }

        _orderStatusId = OrderStatus.Shipped.Id;
        _description = "The order was shipped.";
        
        AddDomainEvent(new OrderShippedEvent(this));
    }
    
    public void SetCancelledStatus()
    {
        if (OrderStatus.Id == OrderStatus.Paid.Id ||
            OrderStatus.Id == OrderStatus.Shipped.Id)
        {
            throw new OrderMgmtException(
                $"Is not possible to change the order status from {OrderStatus.Name} to {OrderStatus.Cancelled.Name}.");
        }

        _orderStatusId = OrderStatus.Cancelled.Id;
        _description = $"The order was cancelled.";
        
        AddDomainEvent(new OrderCancelledEvent(this));
    }
    
    public void SetCancelledStatusWhenStockIsRejected(IEnumerable<Guid> orderStockRejectedItems)
    {
        if (OrderStatus.Id != OrderStatus.AwaitingValidation.Id) return;
        
        _orderStatusId = OrderStatus.Cancelled.Id;

        var itemsStockRejectedProductNames = OrderItems
            .Where(c => orderStockRejectedItems.Contains(c.ProductId))
            .Select(c => c.GetProductName());

        var itemsStockRejectedDescription = string.Join(", ", itemsStockRejectedProductNames);
        _description = $"The product items don't have stock: ({itemsStockRejectedDescription}).";
    }
}