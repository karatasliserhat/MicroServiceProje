using Course.Service.Order.Infrastructure;
using MassTransit;
using MicroService.Shareds.Messages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Service.Order.Application.Consumer
{
    public class CourseNameChangedEventConsume : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CourseNameChangedEventConsume(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItem = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.ProductId).ToListAsync();

            orderItem.ForEach(x =>
            {

                x.UpdateOrderItem(context.Message.UpdateName, x.PictureUrl, x.Price);
            });
            await _orderDbContext.SaveChangesAsync();

        }
    }
}
