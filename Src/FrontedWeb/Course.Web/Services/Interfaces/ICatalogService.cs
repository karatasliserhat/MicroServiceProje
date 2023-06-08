using Course.Web.Models.Courses;

namespace Course.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourseAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string UserId);

        Task<List<CategoryViewModel>> GetAllCategoryAsync();
        Task<CourseViewModel> GetByCourseId(string courseId);
        Task<bool> CreateCourseAsync(CourseCreateInput courseInputModel);
        Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
        Task<bool> DeleteCourseAsync(string CourseId);



    }
}
