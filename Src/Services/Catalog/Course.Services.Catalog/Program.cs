using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services.CategoryServices;
using Course.Services.Catalog.Services.CourseServices;
using Course.Services.Catalog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, conf) =>
    {

        conf.Host(builder.Configuration["RabbitMq"], "/", y =>
        {

            y.Username("guest");
            y.Password("guest");
        });
    });
});

builder.Services.AddOptions<MassTransitHostedService>();
// Add services to the container.

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
{

    opts.RequireHttpsMetadata = false;
    opts.Authority = builder.Configuration["IdentityServerClient"];
    opts.Audience = "resource_catalog";
});
builder.Services.AddControllers(opts =>
{

    opts.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddAutoMapper(typeof(Program));


builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{

    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICourseService, CourseService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scope = app.Services.CreateScope();
var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

if (!categoryService.GetAllAsync().Result.Data.Any())
{
    categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.Net Core" });
    categoryService.CreateAsync(new CategoryCreateDto { Name = "Asp.Net Mvc" });
}
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
