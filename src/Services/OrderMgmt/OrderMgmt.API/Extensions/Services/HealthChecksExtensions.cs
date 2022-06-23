using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace OrderMgmt.API.Extensions.Services;

public static class HealthChecksExtensions
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

        hcBuilder
            .AddNpgSql(
                configuration["OrderMgmtDb"],
                name: "OrderMgmtDb-check",
                tags: new string[] { "ordermgmtdb" });
        
        hcBuilder
            .AddRabbitMQ(
                $"amqp://{configuration["EventBusConnection"]}",
                name: "OrderMgmtDb-RabbitMQBus-check",
                tags: new string[] { "rabbitmqbus" });

        return services;
    }
}