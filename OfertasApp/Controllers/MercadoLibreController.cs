using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using OfertasApp.Helpers; // 👈 Importa TokenManager

namespace OfertasApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MercadoLibreController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenManager _tokenManager;

        public MercadoLibreController(IHttpClientFactory httpClientFactory, TokenManager tokenManager)
        {
            _httpClientFactory = httpClientFactory;
            _tokenManager = tokenManager;
        }

        // Endpoint privado: datos del usuario autenticado
        [HttpGet("me")]
        public async Task<IActionResult> GetUserInfo()
        {
            var token = _tokenManager.GetAccessToken();
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://api.mercadolibre.com/users/me");
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }
}
