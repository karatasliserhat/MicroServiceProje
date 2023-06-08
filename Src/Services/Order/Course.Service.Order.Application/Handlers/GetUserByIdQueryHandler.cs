using Course.Service.Order.Application.DTOs;
using Course.Service.Order.Application.Mappings;
using Course.Service.Order.Application.Queries;
using Course.Service.Order.Infrastructure;
using MediatR;
using MicroService.Shareds.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Course.Service.Order.Application.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByOrderQuery, Response<List<OrderDto>>>
    {

        private readonly OrderDbContext _context;



        public GetUserByIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetUserByOrderQuery request, CancellationToken cancellationToken)
        {
            var dto = await _context.Orders.Include(x=> x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            
            if (!dto.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }
            
            var orderDto = ObjectMapper.mapper.Map<List<OrderDto>>(dto);
            return Response<List<OrderDto>>.Success(orderDto, 200);
        }
    }
}
