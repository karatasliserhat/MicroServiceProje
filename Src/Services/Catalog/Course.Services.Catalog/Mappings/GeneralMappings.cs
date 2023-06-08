using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;

namespace Course.Services.Catalog.Mappings
{
    public class GeneralMappings : Profile
    {
        public GeneralMappings()
        {
            CreateMap<Category, CategoryListDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();


            CreateMap<Coursee, CourseListDto>().ReverseMap();
            CreateMap<Coursee, CourseCreateDto>().ReverseMap();
            CreateMap<Coursee, CourseUpdateDto>().ReverseMap();
        }
    }
}
