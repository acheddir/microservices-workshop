using Microsoft.AspNetCore.Mvc;

namespace OrderMgmt.API.Extensions.Services;

public static class OpenApiVersioningServiceExtension
{
    public static IServiceCollection AddOpenApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            // Default API Version
            config.DefaultApiVersion = new ApiVersion(1, 0);
            // use default version when version is not specified
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        });

        return services;
    }
}