using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderMgmt.Infrastructure;
using SharedKernel.Infrastructure.Common;

namespace OrderMgmt.API.Extensions.Services;

public static class DatabaseServiceExtension
{
    public static IServiceCollection AddOrderMgmtDbContext(this IServiceCollection services, IConfiguration configuration)
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
            }
        );

        services.AddScoped<BaseDbContext, OrderMgmtContext>();
        
        return services;
    }
}