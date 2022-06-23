using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MediatR;
using OrderMgmt.API.Controllers.v1;
using OrderMgmt.API.Middleware;
using OrderMgmt.Application.Services;
using Serilog;
using SharedKernel.Application.Common.Services;
using Sieve.Services;

namespace OrderMgmt.API.Extensions.Services;

public static class ApiServiceExtensions
{
    public static IServiceCollection AddOrderMgmtServices(this IServiceCollection services)
    {
        services.AddSingleton(Log.Logger);
        
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        
        services.AddHttpContextAccessor();
        services.AddMediatR(typeof(Startup));
        services.AddScoped<SieveProcessor>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services
            .AddControllers(o => o.Filters.Add<OrderMgmtErrorHandlerFilterAttribute>())
            .AddFluentValidation(cfg => { cfg.AutomaticValidationEnabled = false; })
            .AddApplicationPart(typeof(OrdersController).Assembly)
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                o.JsonSerializerOptions.WriteIndented = true;
            });

        return services;
    }
}