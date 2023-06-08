using AutoMapper;
using Course.Service.Order.Application.DTOs;
using Course.Services.Order.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Mappings
{
    public class CustomMapperProfile:Profile
    {
        public CustomMapperProfile()
        {
            CreateMap<Adress, AdressDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Services.Order.Domain.OrderAggregate.Order,OrderDto>().ReverseMap();
        }
    }
}
