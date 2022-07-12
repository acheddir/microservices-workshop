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