using Course.Services.FreePayment.Dtos;
using MassTransit;
using MicroService.Shareds.ControllerCustomBases;
using MicroService.Shareds.Dtos;
using MicroService.Shareds.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Course.Services.FreePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreePaymentsController : CustomControllerBase
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FreePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCheckOut(PaymentDto paymentDto)
        {

            var sendEnpont = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-created-service"));

            CreateOrderMessageCommand command = new CreateOrderMessageCommand();

            command.BuyerId = paymentDto.Order.BuyerId;
            command.Adress.Province = paymentDto.Order.Adress.Province;
            command.Adress.Street = paymentDto.Order.Adress.Street;
            command.Adress.District = paymentDto.Order.Adress.District;
            command.Adress.ZipCode = paymentDto.Order.Adress.ZipCode;
            command.Adress.Line = paymentDto.Order.Adress.Line;

            paymentDto.Order.OrderItems.ForEach(x =>
            {
                command.OrderItems.Add(new OrderItem { PictureUrl = x.PictureUrl, Price = x.Price, ProductId = x.ProductId, ProductName = x.ProductName });
            });

             await sendEnpont.Send<CreateOrderMessageCommand>(command);
            return ControllerActionInstanceResult(MicroService.Shareds.Dtos.Response<NoContent>.Success(200));

        }
    }
}
