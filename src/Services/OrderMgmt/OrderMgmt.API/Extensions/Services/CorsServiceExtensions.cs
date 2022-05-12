using OrderMgmt.API.Resources;

namespace OrderMgmt.API.Extensions.Services;

public static class CorsServiceExtensions
{
    public static IServiceCollection AddCorsService(this IServiceCollection services, string policyName, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsEnvironment(Local.IntegrationTestingEnvName) ||
            env.IsEnvironment(Local.FunctionalTestingEnvName))
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder => 
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination"));
            });
        }

        return services;
    }
}