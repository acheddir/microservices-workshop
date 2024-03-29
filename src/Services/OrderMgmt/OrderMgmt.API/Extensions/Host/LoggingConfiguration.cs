﻿namespace OrderMgmt.API.Extensions.Host;

public static class LoggingConfiguration
{
    public static void AddLoggingConfiguration(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var env = services.GetService<IWebHostEnvironment>();

        var loggingLevelSwitch = new LoggingLevelSwitch();
        if (env.IsDevelopment())
            loggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
        if (env.IsProduction())
            loggingLevelSwitch.MinimumLevel = LogEventLevel.Warning;
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(loggingLevelSwitch)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironment(env?.EnvironmentName)
            .Enrich.WithProperty("ApplicationName", env?.ApplicationName!)
            .Enrich.WithExceptionDetails()
            .WriteTo.Console();

        Log.Logger = logger.CreateLogger();
    }
}