using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace OrderMgmt.API.Extensions.Services;

public static class AuthExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.Authority = configuration["Keycloak:Authority"];
            o.Audience = configuration["Keycloak:Audience"];
            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = c =>
                {
                    c.NoResult();

                    c.Response.StatusCode = 500;
                    c.Response.ContentType = "text/plain";
                    return c.Response.WriteAsync(c.Exception.ToString());
                    //return c.Response.WriteAsync("An error occured processing your authentication.");
                }
            };
        });

        services.AddAuthorization(options =>
        {
            //Create policy with more than one claim
            options.AddPolicy("users", 
                policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => (c.Value == "user") || (c.Value == "admin")));
                });
            
            //Create policy with only one claim
            options.AddPolicy("admins",
                policy => policy.RequireClaim(ClaimTypes.Role, "admin"));
            
            //Create a policy with a claim that doesn't exist or you are unauthorized to
            options.AddPolicy("noaccess",
                policy => policy.RequireClaim(ClaimTypes.Role, "noaccess"));
        });

        return services;
    }
}