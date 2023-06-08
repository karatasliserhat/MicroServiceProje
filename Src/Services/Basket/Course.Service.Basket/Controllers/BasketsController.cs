using Course.Service.Basket.Dtos;
using Course.Service.Basket.Services;
using MicroService.Shareds.ControllerCustomBases;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course.Service.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBase()
        {

            return ControllerActionInstanceResult(await _basketService.GetBasketAsync(_sharedIdentityService.GetUserId));
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUser(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;
            return ControllerActionInstanceResult(await _basketService.SaveOrUpddateAsync(basketDto));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return ControllerActionInstanceResult(await _basketService.DeleteBasketAsync(_sharedIdentityService.GetUserId));
        }
    }
}
