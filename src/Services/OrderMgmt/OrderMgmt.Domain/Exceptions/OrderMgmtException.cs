using System.Runtime.Serialization;

namespace OrderMgmt.Domain.Exceptions;

[Serializable]
public class OrderMgmtException: Exception
{
    public OrderMgmtException() { }
    
    public OrderMgmtException(string message)
        : base(message) { }

    public OrderMgmtException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}