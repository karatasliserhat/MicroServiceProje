using Course.Service.Basket.Consumer;
using Course.Service.Basket.Services;
using Course.Service.Basket.Settings;
using MassTransit;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var authnticationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");


//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<CourseNameChangedEventConsume>();
//    x.UsingRabbitMq((context, conf) =>
//    {

//        conf.Host(builder.Configuration["RabbitMq"], "/", x =>
//        {
//            x.Username("guest");
//            x.Password("guest");

//        });

//        conf.ReceiveEndpoint("course-name-changed-baske-service", y =>
//        {
//            y.ConfigureConsumer<CourseNameChangedEventConsume>(context);
//        });
//    });
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{

    opts.Authority = builder.Configuration["IdentityServiceClient"];
    opts.Audience = "resource_basket";
    opts.RequireHttpsMetadata = false;
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddControllers(opts =>
{

    opts.Filters.Add(new AuthorizeFilter(authnticationPolicy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<RedisSetting>(builder.Configuration.GetSection("RedisConnectSettings"));


builder.Services.AddSingleton<RedisService>(sp =>
{
    var service = sp.GetRequiredService<IOptions<RedisSetting>>().Value;

    RedisService redisService = new(service.Host, service.Port);

    redisService.Connect();
    return redisService;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
