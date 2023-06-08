using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public string BuyerId { get; private set; }

        public DateTime CreatedDate { get; private set; }
        private List<OrderItem> _OrderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _OrderItems;

        public Adress Adress { get; private set; }

        public Order()
        {

        }
        public Order(string buyerId, Adress adress)
        {
            _OrderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Adress = adress;
        }
        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            var existOrderItem = _OrderItems.Any(x => x.ProductId == productId);
            if (!existOrderItem)
            {
                var newOrder = new OrderItem(productId, productName, pictureUrl, price);

                _OrderItems.Add(newOrder);
            }
        }

        public decimal GetTotalDecimal => _OrderItems.Sum(x => x.Price);
    }
}
