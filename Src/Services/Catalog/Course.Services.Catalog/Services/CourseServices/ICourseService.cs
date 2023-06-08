using Course.Services.Catalog.Dtos;
using MicroService.Shareds.Dtos;

namespace Course.Services.Catalog.Services.CourseServices
{
    public interface ICourseService
    {
        Task<Response<List<CourseListDto>>> GetAllAsync();
        Task<Response<CourseListDto>> GetByIdAsync(string id);
        Task<Response<List<CourseListDto>>> GetByCourseUserIdAsync(string userId);
        Task<Response<CourseCreateDto>> CreateAsync(CourseCreateDto p);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDto p);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
