using Course.Services.Order.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.DTOs
{
    public  class OrderDto
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string BuyerId { get;  set; }

        public List<OrderItemDto> OrderItems { get; set; }

        public AdressDto Adress { get; private set; }
    }
}
