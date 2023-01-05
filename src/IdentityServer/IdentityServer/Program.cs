using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Add Identity Server4 DI
builder.Services.AddIdentityServer()
     .AddInMemoryClients(Config.Clients)
     .AddInMemoryApiScopes(Config.ApiScopes)     
     .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseRouting();
app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();

