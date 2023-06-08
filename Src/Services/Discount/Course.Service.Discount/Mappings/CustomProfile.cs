using AutoMapper;
using Course.Service.Discount.Dtos;
using Course.Service.Discount.Models;

namespace Course.Service.Discount.Mappings
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<DiscountEntity, DiscountCreateDto>().ReverseMap();
            CreateMap<DiscountEntity, DiscountListDto>().ReverseMap();
            CreateMap<DiscountEntity, DiscountUpdateDto>().ReverseMap();
        }
    }
}
