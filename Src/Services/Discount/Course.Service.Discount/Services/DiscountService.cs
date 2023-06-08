using AutoMapper;
using Course.Service.Discount.Dtos;
using Course.Service.Discount.Interfaces;
using Course.Service.Discount.Models;
using Course.Service.Discount.Repositories;

namespace Course.Service.Discount.Services
{
    public class DiscountService : Repository<DiscountListDto, DiscountCreateDto, DiscountUpdateDto, DiscountEntity>, IDiscountService
    {
        public DiscountService(IConfiguration configuration, IMapper mapper) : base(configuration, mapper)
        {

        }
    }
}
