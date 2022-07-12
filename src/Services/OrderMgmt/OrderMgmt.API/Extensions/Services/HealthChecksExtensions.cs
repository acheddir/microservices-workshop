namespace OrderMgmt.API.Extensions.Services;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        hcBuilder
            .AddNpgSql(
                configuration.GetConnectionString("OrderMgmtDb"),
                name: "OrderMgmtDb-check",
                tags: new string[] { "ordermgmtdb" });
        
        hcBuilder
            .AddRabbitMQ(
                $"amqp://{configuration["EventBusConnection"]}",
                name: "OrderMgmtDb-RabbitMQBus-check",
                tags: new string[] { "rabbitmqbus" });
        
        services
            .AddHealthChecksUI(opt =>
            {
                opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                opt.SetApiMaxActiveRequests(1); //api requests concurrency
                
                opt.AddHealthCheckEndpoint("default api", "/healthz"); //map health check api
            })
            .AddInMemoryStorage();

        return services;
    }
}