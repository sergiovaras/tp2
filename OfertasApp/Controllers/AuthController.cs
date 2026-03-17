using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly MercadoLibreAuthService _authService;

    public AuthController(MercadoLibreAuthService authService)
    {
        _authService = authService;
    }

    // Endpoint para refrescar el token
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        return Ok(result);
    }
    
}
