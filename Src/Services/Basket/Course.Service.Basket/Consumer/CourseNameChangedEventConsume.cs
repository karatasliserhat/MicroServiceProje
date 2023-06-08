using Course.Service.Basket.Dtos;
using Course.Service.Basket.Services;
using MassTransit;
using MicroService.Shareds.Messages;
using MicroService.Shareds.Services;

namespace Course.Service.Basket.Consumer
{
    public class CourseNameChangedEventConsume : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public CourseNameChangedEventConsume(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {

            var basketUser = await _basketService.GetBasketAsync(_sharedIdentityService.GetUserId);

            var basketUpdate =  basketUser.Data.basketItems.Where(x => x.CourseId == context.Message.ProductId).ToList();

            basketUpdate.ForEach(x =>
            {
                x.CourseName = context.Message.UpdateName;
            });
            var basketDto = new BasketDto()
            {
                basketItems = basketUpdate,
                UserId = basketUser.Data.UserId,
                DiscountCode = basketUser.Data.DiscountCode,
                DiscountRate = basketUser.Data.DiscountRate
            };
            await _basketService.SaveOrUpddateAsync(basketDto);
        }
    }
}
