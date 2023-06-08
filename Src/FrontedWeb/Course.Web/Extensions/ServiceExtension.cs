using Course.Web.Handler;
using Course.Web.Services;
using Course.Web.Services.Interfaces;
using Course.Web.Settings;

namespace Course.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddHttpClientService(this WebApplicationBuilder builder)
        {

            var serviceApiSettings = builder.Configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            builder.Services.AddHttpClient<IIdentityService, IdentityService>();
            builder.Services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
            builder.Services.AddHttpClient<ICatalogService, CatalogService>(opts =>
            {
                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            builder.Services.AddHttpClient<IPhotoStockService, PhotoStockService>(opts =>
            {

                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            builder.Services.AddHttpClient<IUserService, UserService>(opts =>
            {

                opts.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();


            builder.Services.AddHttpClient<IBasketService, BasketService>(opts =>
            {

                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IDiscountService, DiscountService>(opts =>
            {

                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IPaymentService, PaymentService>(opts =>
            {

                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.FreePayment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            builder.Services.AddHttpClient<IOrderService, OrderService>(opts =>
            {

                opts.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
