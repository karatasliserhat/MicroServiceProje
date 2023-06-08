using Course.Web.Models.Orders;
using Course.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Course.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;

        public OrderController(IOrderService orderService, IBasketService basketService)
        {
            _orderService = orderService;
            _basketService = basketService;
        }

        public async Task<IActionResult> CheckhOut()
        {
            var basket = await _basketService.GetBasket();
            ViewBag.Basket = basket;

            return View(new CheckOutInfoInput());
        }

        [HttpPost]

        public async Task<IActionResult> CheckhOut(CheckOutInfoInput checkOutInfoInput)
        {
            //yöntem 1 senkron ödeme
            // var responseOrder = await _orderService.CreateOrder(checkOutInfoInput);

            //yöntem 2 asenkron iletişim
            var responseOrder = await _orderService.SuspendOrderCreate(checkOutInfoInput);
            if (!responseOrder.IsSuccessFull)
            {
                var basket = await _basketService.GetBasket();
                ViewBag.Basket = basket;
                ViewBag.Message = responseOrder.Error;
                return View();
            }
            //yöntem 1 senkron
            //return RedirectToAction(nameof(SuccessCheckOut), new { orderId = responseOrder.OrderId });

            //yöntem 2 asenkron iletişim
            return RedirectToAction(nameof(SuccessCheckOut), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessCheckOut(int orderId)
        {
            ViewBag.OrderId = orderId;

            return View();
        }
        public async Task<IActionResult> CheckOutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
