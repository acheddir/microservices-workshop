namespace OrderMgmt.API.Extensions.Services;

public static class CorsExtensions
{
    public static IServiceCollection AddCors(this IServiceCollection services, string policyName, IWebHostEnvironment env)
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
                        .AllowCredentials()
                        .WithExposedHeaders("X-Pagination"));
            });
        }
        else
        {
            //TODO update origins here with env vars or secret
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(policyName, builder =>
            //        builder.WithOrigins(origins)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .WithExposedHeaders("X-Pagination"));
            //});
        }

        return services;
    }
}