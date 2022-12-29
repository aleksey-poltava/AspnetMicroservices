using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging((hostingContext, logging) =>
{
    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));    
    logging.AddConsole();
    logging.AddDebug();
});

//IConfiguration configuration = new ConfigurationBuilder()
//                            .AddJsonFile("ocelot.json")
//                            .Build();

//builder.Services.AddOcelot(configuration);


builder.Services.AddOcelot();

var app = builder.Build();

app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();

