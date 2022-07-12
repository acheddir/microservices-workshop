using OrderMgmt.API.Filters;

namespace OrderMgmt.API.Extensions.Services;

public static class OpenApiExtensions
{
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc(
                "v1.0",
                new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "Order Management API",
                    Description = "Our API uses a REST based design, leverages the JSON data format, and relies upon HTTPS for transport. We respond with meaningful HTTP response codes and if an error occurs, we include error details in the response body. API Documentation is at ordermgmt.com/dev/docs",
                    Contact = new OpenApiContact
                    {
                        Name = "ArcTech",
                        Email = "support@arctech.me",
                        Url = new Uri("https://www.arctech.me"),
                    },
                });
            
            config.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl =
                            new Uri($"{configuration.GetValue<string>("Keycloak:Authority")}/protocol/openid-connect/auth"),
                        TokenUrl =
                            new Uri($"{configuration.GetValue<string>("Keycloak:Authority")}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "ordermgmt", "Order Management Scope" }
                        }
                    }
                }
            });

            config.OperationFilter<AuthorizeCheckOperationFilter>();
            config.OperationFilter<RemoveVersionParameterFilter>();
            config.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            
            config.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}OrderMgmt.API.WebApi.xml"));
        });
        
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