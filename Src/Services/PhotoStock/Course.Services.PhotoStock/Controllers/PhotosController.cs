using Course.Services.PhotoStock.Dtos;
using MicroService.Shareds.ControllerCustomBases;
using MicroService.Shareds.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Course.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreatePhoto(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var newName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos/" + newName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);

                var returnUrl =  newName;

                PhotoDto photoDto = new() { PhotoUrl = returnUrl };

                return ControllerActionInstanceResult(Response<PhotoDto>.Success(photoDto, 200));
            }
            return ControllerActionInstanceResult(Response<NoContent>.Fail("Photo is Empty", 400));
        }
        [HttpDelete]
        public IActionResult DeletePath(string photoUrl)
        {
            var deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Photos/" + photoUrl);

            if (!System.IO.File.Exists(deletePath))
            {
                return ControllerActionInstanceResult(Response<NoContent>.Fail("Photo No File", 404));
            }
            System.IO.File.Delete(deletePath);
            return ControllerActionInstanceResult(Response<NoContent>.Success(404));

        }

    }
}
