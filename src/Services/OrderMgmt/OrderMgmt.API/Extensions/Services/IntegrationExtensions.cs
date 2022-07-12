using SharedKernel.Application.Common.Services;

namespace OrderMgmt.API.Extensions.Services;

public static class IntegrationExtensions
{
    public static IServiceCollection AddIntegration(this IServiceCollection services)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogRepository>>(
            _ => c => new IntegrationEventLogRepository(c));

        services.AddTransient<IIntegrationEventService, OrderMgmtIntegrationEventService>();
        
        return services;
    }
}