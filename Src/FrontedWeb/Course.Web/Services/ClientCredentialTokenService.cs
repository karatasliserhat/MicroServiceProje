using Course.Web.Services.Interfaces;
using Course.Web.Settings;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;

namespace Course.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;

        public ClientCredentialTokenService(ServiceApiSettings serviceApiSettings, ClientSettings clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
        {
            _serviceApiSettings = serviceApiSettings;
            _clientSettings = clientSettings;
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
        }


        public async Task<string> GetTokenAsync()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken", new ClientAccessTokenParameters {  });

            if (currentToken != null)
            {
                return currentToken.AccessToken;
            }

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {

                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var clientCredentialsTokenRequest = new ClientCredentialsTokenRequest
            {

                ClientId = _clientSettings.WebClientApp.ClientId,
                ClientSecret = _clientSettings.WebClientApp.ClientSecret,
                Address = disco.TokenEndpoint
            };
            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);
            if (newToken.IsError)
            {
                throw newToken.Exception;
            }

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn, new ClientAccessTokenParameters { });


            return newToken.AccessToken;
        }
    }
}
