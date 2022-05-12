namespace OrderMgmt.Domain.Exceptions;

public class OrderMgmtException: Exception
{
    public OrderMgmtException(string message) : base(message) { }
}