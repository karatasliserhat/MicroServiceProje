using Course.Service.Basket.Dtos;
using MicroService.Shareds.Dtos;
using StackExchange.Redis;

namespace Course.Service.Basket.Services
{
    public interface IBasketService
    {
          Task<Response<BasketDto>> GetBasketAsync(string UserId);
         Task<Response<bool>> SaveOrUpddateAsync(BasketDto basket);
         Task<Response<bool>> DeleteBasketAsync(string UserId);
    }
}
