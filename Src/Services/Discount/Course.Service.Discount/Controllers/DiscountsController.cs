using Course.Service.Discount.Dtos;
using Course.Service.Discount.Interfaces;
using MicroService.Shareds.ControllerCustomBases;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Course.Service.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedService;
        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedService)
        {
            _discountService = discountService;
            _sharedService = sharedService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscount()
        {
            return ControllerActionInstanceResult(await _discountService.GetAllDiscountAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return ControllerActionInstanceResult(await _discountService.GetDiscountAsync(id));
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetCodeByUser(string code)
        {
            var userid = _sharedService.GetUserId;
            return ControllerActionInstanceResult(await _discountService.GetCodeUserDiscountAsync(code, userid));
        }
        [HttpPost]
        public async Task<IActionResult> CreateDiscount(DiscountCreateDto p)
        {
            return ControllerActionInstanceResult(await _discountService.CreateDiscountAsync(p));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(DiscountUpdateDto p)
        {
            return ControllerActionInstanceResult(await _discountService.UpdateDiscountAsync(p));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            return ControllerActionInstanceResult(await _discountService.DeleteDiscountAsync(id));
        }


    }
}
