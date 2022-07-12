using Microsoft.AspNetCore.Mvc.Filters;
using OrderMgmt.Domain.Exceptions;
using SharedKernel.API.ActionResults;
using SharedKernel.API.Middleware;

namespace OrderMgmt.API.Middleware;

public class OrderMgmtErrorHandlerFilterAttribute : ErrorHandlerFilterAttribute
{
    public OrderMgmtErrorHandlerFilterAttribute(ILogger<OrderMgmtErrorHandlerFilterAttribute> logger, IWebHostEnvironment env)
        : base(logger, env)
    {
        // Register known exception types and handlers.
        ExceptionHandlers?.Add(typeof(OrderMgmtException), HandleOrderMgmtException);
    }

    private void HandleOrderMgmtException(ExceptionContext context)
    {
        var exception = (OrderMgmtException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "An order management domain exception has occured",
            Detail = exception.Message
        };

        context.Result = new InternalServerErrorObjectResult(details);

        context.ExceptionHandled = true;
    }
}