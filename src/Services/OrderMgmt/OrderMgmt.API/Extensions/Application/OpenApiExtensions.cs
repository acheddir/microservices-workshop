using Swashbuckle.AspNetCore.SwaggerUI;

namespace OrderMgmt.API.Extensions.Application;

public static class OpenApiExtensions
{
    public static void UseOpenApi(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(config =>  
        {
            config.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Order Management API v1.0");
            config.DocExpansion(DocExpansion.None);
            config.OAuthAppName("Order Management Swagger UI");
        });
    }
}