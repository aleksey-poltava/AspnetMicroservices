using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));    
    logging.AddConsole();
    logging.AddDebug();
});

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true )
                            .Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

