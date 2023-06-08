using Course.Service.Basket.Dtos;
using MicroService.Shareds.Dtos;
using MicroService.Shareds.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Course.Service.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;
        private readonly ISharedIdentityService _sharedIdentityService;
        public BasketService(RedisService redisService, ISharedIdentityService sharedIdentityService)
        {
            _redisService = redisService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<Response<bool>> DeleteBasketAsync(string UserId)
        {
            var deleteResponse = await _redisService.GetDb().KeyDeleteAsync(UserId);
            return deleteResponse ? Response<bool>.Success(404) : Response<bool>.Fail("could not be done Delete ", 500);
        }

        public async Task<Response<BasketDto>> GetBasketAsync(string UserId)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(UserId);

            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDto>.Fail("Basket Not Found", 404);

            }
            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);

        }

        public async Task<Response<bool>> SaveOrUpddateAsync(BasketDto basket)
        {
            var createResponse = await _redisService.GetDb().StringSetAsync(basket.UserId, JsonSerializer.Serialize(basket));

            return createResponse ? Response<bool>.Success(204) : Response<bool>.Fail("could not be done Save or Update ", 500);
        }

    }
}
