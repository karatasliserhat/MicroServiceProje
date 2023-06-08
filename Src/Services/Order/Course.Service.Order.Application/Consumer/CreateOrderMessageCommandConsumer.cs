using Course.Service.Order.Infrastructure;
using MassTransit;
using MicroService.Shareds.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
           var newAddress = new Services.Order.Domain.OrderAggregate.Adress(context.Message.Adress.Province, context.Message.Adress.District, context.Message.Adress.Street, context.Message.Adress.ZipCode, context.Message.Adress.Line);

            var order = new Services.Order.Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);

            context.Message.OrderItems.ForEach(x =>
            {

                order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price);
            });

            await _orderDbContext.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
