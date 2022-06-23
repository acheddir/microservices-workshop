using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderMgmt.API.Extensions.Application;
using OrderMgmt.API.Extensions.Services;
using OrderMgmt.API.Extensions.Services.EventBus;
using OrderMgmt.Application.Modules;
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
        // TODO: add OpenTelemetry
        
        services
            .AddOrderMgmtServices()
            .AddIntegration()
            .AddEventBus()
            .AddDatabases(_config)
            .AddCors("OrderMgmtCorsPolicy", _env)
            .AddOpenApi(_config)
            .AddHealthChecks(_config);
    }
    
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule<MediatorModule>();
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

        app.UseRouting();
        app.UseCors("OrderMgmtCorsPolicy");

        app.UseSerilogRequestLogging();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });
            endpoints.MapControllers();
            
            endpoints.Redirect("/", "/swagger");
        });
        
        app.UseOpenApi(_config);
    }
}