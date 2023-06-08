using Course.Services.Catalog.Dtos;
using MicroService.Shareds.Dtos;

namespace Course.Services.Catalog.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryListDto>>> GetAllAsync();
        Task<Response<CategoryListDto>> GetByIdAsync(string id);
        Task<Response<CategoryCreateDto>> CreateAsync(CategoryCreateDto p);
        Task<Response<NoContent>> DeleteAsync(string id);
        Task<Response<NoContent>> UpdateAsync(CategoryUpdateDto p);
    }
}
