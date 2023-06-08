using Course.Gateway.DelegetingHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile($"configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json".ToLower()).AddEnvironmentVariables();
});
builder.Services.AddHttpClient<TokenExchangeDelegetingHandler>();
builder.Services.AddAuthentication().AddJwtBearer("GatewayAuhenticationSchema", opts =>
{
    opts.Authority = builder.Configuration["IdentityServiceConnect"];
    opts.Audience = "resource_gateway";
    opts.RequireHttpsMetadata = false;
});
// Add services to the container.

builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegetingHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseOcelot().Wait();
app.UseAuthorization();

app.Run();



