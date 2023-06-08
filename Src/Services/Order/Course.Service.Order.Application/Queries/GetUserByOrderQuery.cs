using Course.Service.Order.Application.DTOs;
using MediatR;
using MicroService.Shareds.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Queries
{
    public class GetUserByOrderQuery : IRequest<Response<List<OrderDto>>>
    {

        public string UserId { get; set; }

        public GetUserByOrderQuery(string userId)
        {
            UserId=userId;
        }

        


    }
}
