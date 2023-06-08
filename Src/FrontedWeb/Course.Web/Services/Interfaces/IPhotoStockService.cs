using Course.Web.Models.PhotoStocks;

namespace Course.Web.Services.Interfaces
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> PhotoSave(IFormFile photoUrl);
        Task<bool> PhotoDelete(string photoUrl);
    }
}
