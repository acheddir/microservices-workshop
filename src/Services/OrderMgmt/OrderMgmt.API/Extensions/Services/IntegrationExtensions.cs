using System.Data.Common;
using OrderMgmt.Application.Services;
using SharedKernel.Application.Common.Services;
using SharedKernel.EventBus.Services;

namespace OrderMgmt.API.Extensions.Services;

public static class IntegrationExtensions
{
    public static IServiceCollection AddIntegration(this IServiceCollection services)
    {
        services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
            _ => c => new IntegrationEventLogService(c));

        services.AddTransient<IIntegrationEventService, OrderMgmtIntegrationEventService>();
        
        return services;
    }
}