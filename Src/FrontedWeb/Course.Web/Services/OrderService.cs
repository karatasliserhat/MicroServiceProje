using Course.Web.Models.FakePayments;
using Course.Web.Models.Orders;
using Course.Web.Services.Interfaces;
using MicroService.Shareds.Dtos;
using MicroService.Shareds.Services;

namespace Course.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IBasketService _baskerService;
        private readonly IPaymentService _paymentService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(HttpClient httpClient, IBasketService baskerService, IPaymentService paymentService, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient;
            _baskerService = baskerService;
            _paymentService = paymentService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await _baskerService.GetBasket();

            var paymentInfoInput = new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CardNumber = checkOutInfoInput.CardNumber,
                CVV = checkOutInfoInput.CVV,
                Expiration = checkOutInfoInput.Expiration,
                TotalPrice = basket.TotalPriace
            };

            var responsePayment = await _paymentService.ReceivedPayment(paymentInfoInput);
            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme İşlemi Başarısız", IsSucessFul = false };
            }

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Adress = new AdressCreateInput { Province = checkOutInfoInput.Province, District = checkOutInfoInput.District, Line = checkOutInfoInput.Line, Street = checkOutInfoInput.Street, ZipCode = checkOutInfoInput.ZipCode },

            };

            basket.BasketItems.ForEach(x =>
            {

                var orderItemCreateInput = new OrderItemCreateInput { PictureUrl = "", Price = x.GetCurrentPrice, ProductId = x.CourseId, ProductName = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItemCreateInput);
            });

            var responseOrder = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (!responseOrder.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel { Error = "Sipariş Oluşturulamadı", IsSucessFul = false };
            }

            var readOrder = await responseOrder.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

            readOrder.Data.IsSucessFul = true;

            await _baskerService.Delete();
            return readOrder.Data;

        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var responseGetOrder = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return responseGetOrder.Data;
        }

        public async Task<SuspendCreateViewModel> SuspendOrderCreate(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await _baskerService.GetBasket();

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Adress = new AdressCreateInput { Province = checkOutInfoInput.Province, District = checkOutInfoInput.District, Line = checkOutInfoInput.Line, Street = checkOutInfoInput.Street, ZipCode = checkOutInfoInput.ZipCode },

            };

            basket.BasketItems.ForEach(x =>
            {

                var orderItemCreateInput = new OrderItemCreateInput { PictureUrl = "", Price = x.GetCurrentPrice, ProductId = x.CourseId, ProductName = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItemCreateInput);
            });

            var paymentInfoInput = new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CardNumber = checkOutInfoInput.CardNumber,
                CVV = checkOutInfoInput.CVV,
                Expiration = checkOutInfoInput.Expiration,
                TotalPrice = basket.TotalPriace,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivedPayment(paymentInfoInput);
            if (!responsePayment)
            {
                return new SuspendCreateViewModel() { Error = "Ödeme İşlemi Başarısız", IsSuccessFull = false };
            }
            await _baskerService.Delete();
            return new SuspendCreateViewModel() { IsSuccessFull = true };
        }
    }
}
