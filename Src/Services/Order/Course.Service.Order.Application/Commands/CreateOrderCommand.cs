using Course.Service.Order.Application.DTOs;
using MediatR;
using MicroService.Shareds.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Commands
{
    public  class CreateOrderCommand:IRequest<Response<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public AdressDto Adress { get; set; }
    }
}
