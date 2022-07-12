namespace OrderMgmt.API.Extensions.Services.EventBus;

public static class EventBusExtensions
{
    public static IServiceCollection AddEventBus(this IServiceCollection services)
    {
        services.AddMassTransit(mt =>
        {
            mt.AddConsumers(Assembly.GetExecutingAssembly());
            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(Environment.GetEnvironmentVariable("RMQ_HOST"), Environment.GetEnvironmentVariable("RMQ_VIRTUAL_HOST"), h =>
                {
                    h.Username(Environment.GetEnvironmentVariable("RMQ_USERNAME"));
                    h.Password(Environment.GetEnvironmentVariable("AUTH_PASSWORD"));
                });

                // Producers
                cfg.RegisterProducers();
                // Consumers
                cfg.RegisterConsumers(context);
            });
        });
        services.AddOptions<MassTransitHostOptions>();
        
        return services;
    }
}