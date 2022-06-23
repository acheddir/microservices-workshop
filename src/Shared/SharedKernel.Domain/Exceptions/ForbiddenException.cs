using System.Runtime.Serialization;

namespace SharedKernel.Domain.Exceptions;

[Serializable]
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() { }
    public ForbiddenAccessException(string message) : base(message) { }
    public ForbiddenAccessException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }
}