using Course.Service.Discount.Dtos;
using Course.Service.Discount.Models;
using MicroService.Shareds.Dtos;

namespace Course.Service.Discount.Interfaces
{
    public interface IRepository<ListDto, CreateDto, UpdateDto, T> where ListDto : BaseDto where CreateDto : BaseDto where UpdateDto : BaseDto where T : BaseEntity
    {
        Task<Response<List<ListDto>>> GetAllDiscountAsync();
        Task<Response<ListDto>> GetDiscountAsync(int id);

        Task<Response<ListDto>> GetCodeUserDiscountAsync(string code, string userId);
        Task<Response<CreateDto>> CreateDiscountAsync(CreateDto entity);

        Task<Response<NoContent>> UpdateDiscountAsync(UpdateDto entity);
        Task<Response<NoContent>> DeleteDiscountAsync(int id);

    }
}
