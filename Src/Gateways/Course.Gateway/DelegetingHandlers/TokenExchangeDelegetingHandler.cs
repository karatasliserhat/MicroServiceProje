using IdentityModel.Client;

namespace Course.Gateway.DelegetingHandlers
{
    public class TokenExchangeDelegetingHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public TokenExchangeDelegetingHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string _access_token;

        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_access_token))
            {
                return _access_token;
            }
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServiceConnect"],
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }


            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrandType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token",
                Scope = "openid payment_fullpermission discount_fullpermission"
            };

            var tokenResult = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResult.IsError)
            {
                throw tokenResult.Exception;
            }

            _access_token = tokenResult.AccessToken;

            return _access_token;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;
            var newToken = await GetToken(requestToken);
            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
