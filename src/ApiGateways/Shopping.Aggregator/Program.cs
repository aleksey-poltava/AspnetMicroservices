using System;
using Common.Logging;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shopping.Aggregator.Poicies;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

//Add health check
var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build();

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri($"{config.GetSection("ApiSettings:CatalogUrl").Value }/swagger/index.html"), "Catalog.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{config.GetSection("ApiSettings: BasketUrl") }/swagger/index.html"), "Basket.API", HealthStatus.Degraded)
    .AddUrlGroup(new Uri($"{config.GetSection("ApiSettings: OrderingUrl") }/swagger/index.html"), "Ordering.API", HealthStatus.Degraded);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Get Logger
var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
}).CreateLogger("Program");

logger.LogInformation("Shopping aggregator is starting");


//register HttpClients for IHttpClientFactory
builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PolicyManager.GetRetryPolicy(logger))
    .AddPolicyHandler(PolicyManager.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IBasketService, BasketService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PolicyManager.GetRetryPolicy(logger))
    .AddPolicyHandler(PolicyManager.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(PolicyManager.GetRetryPolicy(logger))
    .AddPolicyHandler(PolicyManager.GetCircuitBreakerPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

