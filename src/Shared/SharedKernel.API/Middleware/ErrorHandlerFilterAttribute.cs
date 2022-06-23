using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.API.ActionResults;
using SharedKernel.Domain.Exceptions;

namespace SharedKernel.API.Middleware;

public abstract class ErrorHandlerFilterAttribute : ExceptionFilterAttribute
{
    protected readonly IDictionary<Type, Action<ExceptionContext>> ExceptionHandlers;

    protected readonly ILogger Logger;
    protected readonly IWebHostEnvironment Env;

    protected ErrorHandlerFilterAttribute(ILogger<ErrorHandlerFilterAttribute> logger, IWebHostEnvironment env)
    {
        Logger = logger;
        Env = env;

        // Register known exception types and handlers.
        ExceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(FluentValidation.ValidationException), HandleFluentValidationException },
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        var message = context.Exception.Message;
        
        Logger.LogError(new EventId(context.Exception.HResult), context.Exception, message);
        
        HandleException(context);

        base.OnException(context);
    }
    
    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (ExceptionHandlers.ContainsKey(type))
        {
            ExceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }
    
    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        HandleErrors(context, exception.Errors);
    }
    
    private void HandleFluentValidationException(ExceptionContext context)
    {
        var exception = (FluentValidation.ValidationException)context.Exception;
        var failures = exception.Errors
            .ToList();
        var proper = new ValidationException(failures);
        
        HandleErrors(context, proper.Errors);
    }
    
    private static void HandleErrors(ExceptionContext context, IDictionary<string, string[]> errors)
    {
        var details = new ValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details)
        {
            ContentTypes = { "application/problem+json", "application/problem+xml" }
        };

        context.ExceptionHandled = true;
    }
    
    private static void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details)
        {
            ContentTypes = { "application/problem+json", "application/problem+xml" }
        };

        context.ExceptionHandled = true;
    }
    
    private static void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var json = new JsonErrorResponse
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Messages = new[] { "An error occured. We're investigating the cause of that, please try again later :)" }
        };

        if (Env.IsDevelopment())
        {
            json.DeveloperMessage = context.Exception;
        }

        // Result assigned to a result object but in destiny the response is empty. This is a known bug of .net core 1.1
        // It will be fixed in .net core 1.1.2. See https://github.com/aspnet/Mvc/issues/5594 for more information
        context.Result = new InternalServerErrorObjectResult(json);
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        context.ExceptionHandled = true;
    }
    
    private class JsonErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Type { get; set; }
        public string[]? Messages { get; set; }
        public object? DeveloperMessage { get; set; }
    }
}