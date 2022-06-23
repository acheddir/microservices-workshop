using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderMgmt.Infrastructure;
using SharedKernel.EventBus;
using SharedKernel.Infrastructure.Common;

namespace OrderMgmt.API.Extensions.Services;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderMgmtContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("OrderMgmtDb"),
                    npgsqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(OrderMgmtContext).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions
                            .EnableRetryOnFailure(
                                maxRetryCount: 15,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorCodesToAdd: null);
                    }).UseSnakeCaseNamingConvention();
            },
            ServiceLifetime.Scoped
        );
        
        services.AddDbContext<IntegrationEventLogContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("OrderMgmtDb"),
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(IntegrationEventLogContext).GetTypeInfo().Assembly.GetName().Name);
                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<BaseDbContext, OrderMgmtContext>();
        
        return services;
    }
}