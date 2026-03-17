using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace OfertasApp.Helpers
{
    public class TokenManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private TokenResponse? _token;

        private readonly string _clientId = "4830097416227933";
        private readonly string _clientSecret = "sc0c7MhCByc0VuFOyJyg2ll97BK7TbXk";

        public TokenManager(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public string GetAccessToken()
        {
            if (_token == null || DateTime.UtcNow >= _token.ExpirationTime)
            {
                RefreshTokenAsync().GetAwaiter().GetResult();
            }
            return _token!.AccessToken;
        }

        public void SetInitialToken(TokenResponse token)
        {
            token.ExpirationTime = DateTime.UtcNow.AddSeconds(token.ExpiresIn);
            _token = token;
        }

        private async Task RefreshTokenAsync()
        {
            if (_token == null) throw new InvalidOperationException("No hay token inicial para refrescar.");

            var values = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "refresh_token", _token.RefreshToken }
            };

            var content = new FormUrlEncodedContent(values);
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", content);
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var newToken = JsonSerializer.Deserialize<TokenResponse>(json, options);

            if (newToken != null)
            {
                newToken.ExpirationTime = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn);
                _token = newToken;
            }
        }
    }
}
