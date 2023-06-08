using Course.Service.Order.Application.Commands;
using Course.Service.Order.Application.Queries;
using MediatR;
using MicroService.Shareds.ControllerCustomBases;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course.Services.Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomControllerBase
    {
        private readonly IMediator _mediator1;
        private readonly ISharedIdentityService _sharedIdentityService;


        public OrdersController(IMediator mediator, ISharedIdentityService identityService)
        {
            _mediator1 = mediator;
            _sharedIdentityService = identityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUserByOrder()
        {
            var userOrder = await _mediator1.Send(new GetUserByOrderQuery(_sharedIdentityService.GetUserId));

            return ControllerActionInstanceResult(userOrder);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderCommand createOrderCommand)
        {
            var response = await _mediator1.Send(createOrderCommand);

            return ControllerActionInstanceResult(response);
        }

    }

}
