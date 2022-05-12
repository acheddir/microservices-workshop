using System.Runtime.Serialization;
using FluentValidation.Results;

namespace SharedKernel.Exceptions;

[Serializable]
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
    
    public ValidationException(SerializationInfo info, StreamingContext ctx) : base(info, ctx) { }

    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
}