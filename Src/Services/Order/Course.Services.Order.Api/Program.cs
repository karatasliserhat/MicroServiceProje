using Course.Service.Order.Application.Consumer;
using Course.Service.Order.Infrastructure;
using MassTransit;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{

    x.AddConsumer<CreateOrderMessageCommandConsumer>();
    x.AddConsumer<CourseNameChangedEventConsume>();
    x.UsingRabbitMq((context, conf) =>
    {

        conf.Host(builder.Configuration["RabbitMq"], "/", y =>
        {

            y.Username("guest");
            y.Password("guest");

        });
        conf.ReceiveEndpoint("order-created-service", z =>
        {

            z.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
        });
        conf.ReceiveEndpoint("course-name-changed-order-service", m =>
        {

            m.ConfigureConsumer<CourseNameChangedEventConsume>(context);
        });
    });
});
builder.Services.AddOptions<MassTransitHostedService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{
    opts.Authority = builder.Configuration["IdentityServerConnect"];
    opts.Audience = "resource_order";
    opts.RequireHttpsMetadata = false;
});
// Add services to the container.
var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

builder.Services.AddDbContext<OrderDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("Course.Service.Order.Infrastructure");
    });
});
builder.Services.AddControllers(conf =>
{

    conf.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblyContaining(typeof(Course.Service.Order.Application.Handlers.CreateOrderCommandHandler));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var orderDbContext=serviceProvider.GetRequiredService<OrderDbContext>();
    orderDbContext.Database.Migrate();
}
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
