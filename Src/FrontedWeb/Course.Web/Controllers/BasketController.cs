using Course.Web.Models.Baskets;
using Course.Web.Models.Discounts;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Course.Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {

        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasket());
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseId(courseId);

            var basketItem = new BasketItemViewModel() { CourseId = courseId, CourseName = course.Name, Priace = course.Price };

            await _basketService.AddBasketItem(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteBasketItem(string courseId)
        {
            await _basketService.DeleteBasketItem(courseId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscountCode(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountErrorMessage"] = ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var resultCode = await _basketService.ApplyDiscount(discountApplyInput.Code);
            TempData["discountCode"] = resultCode;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CancellationCode()
        {
            await _basketService.CancelApplyDiscount();
            return RedirectToAction(nameof(Index));
        }
    }
}
