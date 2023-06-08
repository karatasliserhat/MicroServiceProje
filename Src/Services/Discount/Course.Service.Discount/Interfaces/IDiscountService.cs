using Course.Service.Discount.Dtos;
using Course.Service.Discount.Models;

namespace Course.Service.Discount.Interfaces
{
    public interface IDiscountService : IRepository<DiscountListDto, DiscountCreateDto, DiscountUpdateDto, DiscountEntity>
    {
    }
}
