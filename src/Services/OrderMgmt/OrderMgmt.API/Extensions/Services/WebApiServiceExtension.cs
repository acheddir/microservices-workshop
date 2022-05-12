using System.Reflection;
using FluentValidation.AspNetCore;
using MediatR;
using OrderMgmt.API.Middleware;
using OrderMgmt.Application.Services;
using SharedKernel.Application.Common.Services;
using Sieve.Services;

namespace OrderMgmt.API.Extensions.Services;

public static class WebApiServiceExtension
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddMediatR(typeof(Startup));
        services.AddScoped<SieveProcessor>();
        services
            .AddMvc(options => options.Filters.Add<ErrorHandlerFilterAttribute>())
            .AddFluentValidation(cfg => { cfg.AutomaticValidationEnabled = false; });
        
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}