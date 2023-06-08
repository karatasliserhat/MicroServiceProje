using Course.Web.Settings;

namespace Course.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(ServiceApiSettings serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings;
        }

        public string GetPhotoUrl(string photoUrl)
        {
            return $"{_serviceApiSettings.PhotoStockUri}/Photos/{photoUrl}";
        }
    }
}
