using MediatR;

namespace SharedKernel.Application.Common;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommand<out TRequest> : IRequest<TRequest> 
{
}