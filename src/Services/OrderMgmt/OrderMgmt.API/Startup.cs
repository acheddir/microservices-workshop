using System.Text.Json.Serialization;
using OrderMgmt.API.Extensions.Application;
using OrderMgmt.API.Extensions.Services;
using Serilog;

namespace OrderMgmt.API;

public class Startup
{
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        _config = configuration;
        _env = env;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(Log.Logger);

        services.AddOrderMgmtDbContext(_config)
            .AddCorsService("OrderMgmtCorsPolicy", _env)
            .AddOpenApi(_config)
            .AddOpenApiVersioning()
            .AddWebApiServices()
            .AddHealthChecks();
        
        services
            .AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        
        services.AddHealthChecks();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (_env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // For elevated security, it is recommended to remove this middleware and set your server to only listen on https.
        // A slightly less secure option would be to redirect http to 400, 505, etc.
        app.UseHttpsRedirection();

        app.UseCors("OrderMgmtCorsPolicy");

        app.UseSerilogRequestLogging();
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/api/health");
            endpoints.MapControllers();
            
            endpoints.Redirect("/", "/swagger");
        });
        
        app.UseOpenApi(_config);
    }
}