using Course.Service.Order.Application.Commands;
using Course.Service.Order.Application.DTOs;
using Course.Service.Order.Infrastructure;
using Course.Services.Order.Domain.OrderAggregate;
using MediatR;
using MicroService.Shareds.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Adress adress = new Adress(request.Adress.Province, request.Adress.District, request.Adress.Street, request.Adress.ZipCode, request.Adress.Line);


            Services.Order.Domain.OrderAggregate.Order newOrder = new Services.Order.Domain.OrderAggregate.Order(request.BuyerId, adress);

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price);
            });
            await _context.Orders.AddAsync(newOrder);

            await _context.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }
    }
}
