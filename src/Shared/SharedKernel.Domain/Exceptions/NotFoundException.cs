using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions;

[Serializable]
public class NotFoundException : Exception
{
    public NotFoundException()
        : base()
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
    
    public NotFoundException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
}