using Course.Web.Models;
using Course.Web.Services.Interfaces;
using Course.Web.Settings;
using IdentityModel.Client;
using MicroService.Shareds.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace Course.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient httpClient, IHttpContextAccessor contextAccessor, ClientSettings clientSettings, ServiceApiSettings serviceApiSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _clientSettings = clientSettings;
            _serviceApiSettings = serviceApiSettings;
        }

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.RefreshToken);
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest
            {

                Address = disco.TokenEndpoint,
                RefreshToken = refreshToken,
                ClientId = _clientSettings.WebClientAppForUser.ClientId,
                ClientSecret = _clientSettings.WebClientAppForUser.ClientSecret,

            };

            var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            if (token.IsError)
            {
                throw token.Exception;
            }

            var authenticationProperties = new List<AuthenticationToken>() {

             new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
             new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
             new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)},

            };

            var authenticationResult = await _contextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;
            properties.StoreTokens(authenticationProperties);

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });
            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var refreshToken = await _contextAccessor.HttpContext.GetTokenAsync(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new TokenRevocationRequest
            {
                Address = disco.RevocationEndpoint,
                ClientId = _clientSettings.WebClientAppForUser.ClientId,
                ClientSecret = _clientSettings.WebClientAppForUser.ClientSecret,
                Token = refreshToken,
                TokenTypeHint = "refresh_token"
            };

            var revokeToken = await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SignInInput signInInput)
        {
            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            var passwordTokenRequest = new PasswordTokenRequest
            {

                ClientId = _clientSettings.WebClientAppForUser.ClientId,
                ClientSecret = _clientSettings.WebClientAppForUser.ClientSecret,
                UserName = signInInput.Email,
                Password = signInInput.Password,
                Address = disco.TokenEndpoint
            };

            var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return Response<bool>.Fail(errorDto.Errors, 400);


            }

            var userInfoRequest = new UserInfoRequest
            {
                Token = token.AccessToken,
                Address = disco.UserInfoEndpoint
            };

            var userinfo = await _httpClient.GetUserInfoAsync(userInfoRequest);

            if (userinfo.IsError)
            {
                throw userinfo.Exception;
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userinfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperty = new AuthenticationProperties
            {
                IsPersistent = signInInput.IsRemember
            };

            authenticationProperty.StoreTokens(new List<AuthenticationToken> {

                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.AccessToken, Value=token.AccessToken},
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.RefreshToken, Value=token.RefreshToken},
                new AuthenticationToken(){ Name=OpenIdConnectParameterNames.ExpiresIn, Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}

                });
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperty);

            return Response<bool>.Success(200);
        }
    }
}
