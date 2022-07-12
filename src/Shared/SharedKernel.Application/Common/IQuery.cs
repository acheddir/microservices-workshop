namespace SharedKernel.Application.Common;

public interface IQuery<out TRequest> : IRequest<TRequest>
{
}