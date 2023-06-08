using Course.Web.Extensions;
using Course.Web.Handler;
using Course.Web.Helpers;
using Course.Web.Settings;
using Course.Web.Validator;
using FluentValidation.AspNetCore;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(fv=> {
    fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<CourseUpdateInputValidator>();
    fv.RegisterValidatorsFromAssemblyContaining<DiscountApplyInputValidator>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddAccessTokenManagement();
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));
builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));


builder.AddHttpClientService();


builder.Services.AddSingleton<ServiceApiSettings>(sp =>
{
    var service = sp.GetRequiredService<IOptions<ServiceApiSettings>>().Value;

    return service;
});
builder.Services.AddSingleton<ClientSettings>(sp =>
{
    var service = sp.GetRequiredService<IOptions<ClientSettings>>().Value;
    return service;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
{

    opts.LoginPath = new PathString("/Auth/SignIn");
    opts.ExpireTimeSpan = TimeSpan.FromDays(60);
    opts.SlidingExpiration = true;
    opts.Cookie.Name = "coursecookie";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
