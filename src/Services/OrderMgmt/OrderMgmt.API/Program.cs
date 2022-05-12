using System.Reflection;
using OrderMgmt.API;
using OrderMgmt.API.Extensions.Host;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup(typeof(Startup).GetTypeInfo().Assembly.FullName!)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseKestrel();
    }).Build();

host.AddLoggingConfiguration();

try
{
    Log.Information("Starting application");
    await host.RunAsync();
}
catch (Exception e)
{
    Log.Error(e, "The application failed to start correctly");
    throw;
}
finally
{
    Log.Information("Shutting down application");
    Log.CloseAndFlush();
}
