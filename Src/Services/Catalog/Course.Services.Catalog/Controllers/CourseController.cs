using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services.CourseServices;
using MicroService.Shareds.ControllerCustomBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _courseService.GetAllAsync();

            return ControllerActionInstanceResult(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _courseService.GetByIdAsync(id);
            return ControllerActionInstanceResult(data);
        }

        [HttpGet]

        [Route("/api/[controller]/GetAllCourseUserId/{userId}")]

        public async Task<IActionResult> GetAllCourseUserId(string userId)
        {
            var data = await _courseService.GetByCourseUserIdAsync(userId);
            return ControllerActionInstanceResult(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto p)
        {
            var result = await _courseService.CreateAsync(p);
            return ControllerActionInstanceResult(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto p)
        {
            var result = await _courseService.UpdateAsync(p);
            return ControllerActionInstanceResult(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _courseService.DeleteAsync(id);
            return ControllerActionInstanceResult(result);
        }
    }
}
