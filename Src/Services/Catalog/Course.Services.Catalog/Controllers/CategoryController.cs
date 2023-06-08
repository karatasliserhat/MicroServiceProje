using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services.CategoryServices;
using MicroService.Shareds.ControllerCustomBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _categoryService.GetAllAsync();

            return ControllerActionInstanceResult(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _categoryService.GetByIdAsync(id);
            return ControllerActionInstanceResult(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto p)
        {
            var result = await _categoryService.CreateAsync(p);
            return ControllerActionInstanceResult(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update(CategoryUpdateDto p)
        {
            var result = await _categoryService.UpdateAsync(p);
            return ControllerActionInstanceResult(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return ControllerActionInstanceResult(result);
        }
    }
}
