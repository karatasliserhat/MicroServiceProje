using Course.Web.Models.PhotoStocks;
using Course.Web.Services.Interfaces;
using MicroService.Shareds.Dtos;

namespace Course.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _client;

        public PhotoStockService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> PhotoDelete(string photoUrl)
        {
            var response = await _client.DeleteAsync($"photos?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> PhotoSave(IFormFile photoUrl)
        {
            if (photoUrl != null && photoUrl.Length > 0)
            {

                var newName = $"{Guid.NewGuid()}{Path.GetExtension(photoUrl.FileName)}";

                var memoryStream = new MemoryStream();

                await photoUrl.CopyToAsync(memoryStream);

                var multipartFormDataContent = new MultipartFormDataContent();

                multipartFormDataContent.Add(new ByteArrayContent(memoryStream.ToArray()), "photo", newName);

                var response = await _client.PostAsync("photos", multipartFormDataContent);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                var responseSuccess = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
                return responseSuccess.Data;
            }
            else
            {
                return null;

            }




        }
    }
}
