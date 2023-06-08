using Course.Web.Models.Courses;
using Course.Web.Services.Interfaces;
using MicroService.Shareds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;

namespace Course.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IPhotoStockService _photoStockService;
        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService, IPhotoStockService photoStockService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
            _photoStockService = photoStockService;
        }

        public async Task<IActionResult> UserCourse()
        {


            return View(await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var responsePhotoUrl = await _photoStockService.PhotoSave(courseCreateInput.PhotoUrlFile);
            if (responsePhotoUrl != null)
            {
                courseCreateInput.Picture = responsePhotoUrl.PhotoUrl;
            }
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");

            if (ModelState.IsValid)
            {
                courseCreateInput.UserId = _sharedIdentityService.GetUserId;
                var response = await _catalogService.CreateCourseAsync(courseCreateInput);
                return RedirectToAction(nameof(UserCourse));
            }
            return View();
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetByCourseId(id);
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);

            if (course == null)
            {
                RedirectToAction(nameof(UserCourse));
            }
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Price = course.Price,
                Feature = course.Feature,
                CategoryId = course.CategoryId,
                Description = course.Description,
                UserId = course.UserId,
                Picture = course.Picture
            };
            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            if (courseUpdateInput.PhotoUrlFile!=null && courseUpdateInput.PhotoUrlFile.Length>0)
            {
                var responsePicture = await _photoStockService.PhotoSave(courseUpdateInput.PhotoUrlFile);
                if (responsePicture != null)
                {
                    await _photoStockService.PhotoDelete(courseUpdateInput.Picture);
                    courseUpdateInput.Picture = responsePicture.PhotoUrl;
                }
            }
           
           
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.CategoryId);
            if (!ModelState.IsValid)
            {
                return View();
            }
            var response = await _catalogService.UpdateCourseAsync(courseUpdateInput);

            return RedirectToAction(nameof(UserCourse));
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View();
            }
            var course = await _catalogService.GetByCourseId(id);
            await _photoStockService.PhotoDelete(course.Picture);
            await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(UserCourse));

        }
    }
}
