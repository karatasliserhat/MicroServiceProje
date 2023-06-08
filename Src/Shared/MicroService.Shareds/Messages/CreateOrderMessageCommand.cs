using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace MicroService.Shareds.Messages
{
    public class CreateOrderMessageCommand
    {
        public CreateOrderMessageCommand()
        {
            OrderItems = new List<OrderItem>();
            Adress=new AdressDto();
        }
        public string BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public AdressDto Adress { get; set; }
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public Decimal Price { get; set; }
    }
    public class AdressDto
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }

}
