using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OfertasApp.Helpers; // 👈 Importa el namespace donde está TokenResponse y TokenManager

namespace OfertasApp.Controllers
{
    [ApiController]
    [Route("callback")]
    public class CallbackController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenManager _tokenManager;

        public CallbackController(IHttpClientFactory httpClientFactory, TokenManager tokenManager)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public async Task<IActionResult> Receive([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("No se recibió el code");

            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", "4830097416227933" },
                { "client_secret", "sc0c7MhCByc0VuFOyJyg2ll97BK7TbXk" },
                { "code", code },
                { "redirect_uri", "https://doretta-rugulose-cicely.ngrok-free.dev/callback" }
            };

            var content = new FormUrlEncodedContent(values);
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync("https://api.mercadolibre.com/oauth/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseString, options);

            if (tokenResponse != null)
            {
                tokenResponse.ExpirationTime = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
                _tokenManager.SetInitialToken(tokenResponse);
                return Ok(tokenResponse);
            }

            return BadRequest(responseString);
        }
    }
}
