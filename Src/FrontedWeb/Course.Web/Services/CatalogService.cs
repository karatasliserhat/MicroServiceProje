using Course.Web.Helpers;
using Course.Web.Models.Courses;
using Course.Web.Services.Interfaces;
using MicroService.Shareds.Dtos;
using System.Net.Http.Json;

namespace Course.Web.Services
{


    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly PhotoHelper _photoHelper;
        public CatalogService(HttpClient httpClient, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var responseCreate = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);

            return responseCreate.IsSuccessStatusCode;

        }

        public async Task<bool> DeleteCourseAsync(string CourseId)
        {
            var responseDelete = await _httpClient.DeleteAsync($"course/{CourseId}");

            return responseDelete.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            var response = await _httpClient.GetAsync("category");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CategoryViewModel> { };
            }
            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();

            return responseData.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            var response = await _httpClient.GetAsync("course");
            if (!response.IsSuccessStatusCode)
            {
                return new List<CourseViewModel> { };
            }
            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseData.Data.ForEach(x =>
            {
                x.StockPhotoUrl = _photoHelper.GetPhotoUrl(x.Picture);
            });
            return responseData.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"course/GetAllCourseUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return new List<CourseViewModel> { };

            }

            var responseData = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseData.Data.ForEach(x =>
            {
                x.StockPhotoUrl = _photoHelper.GetPhotoUrl(x.Picture);
            });
            return responseData.Data;
        }

        public async Task<CourseViewModel> GetByCourseId(string courseId)
        {
            var response = await _httpClient.GetAsync($"course/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseData = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();

            responseData.Data.StockPhotoUrl = _photoHelper.GetPhotoUrl(responseData.Data.Picture);
            return responseData.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var responseUpdate = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("course", courseUpdateInput);
            return responseUpdate.IsSuccessStatusCode;
        }
    }
}
