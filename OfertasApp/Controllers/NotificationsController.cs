using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("notifications")]
public class NotificationsController : ControllerBase
{
    [HttpPost]
    public IActionResult Receive([FromBody] JsonElement payload)
    {
        // Procesar la notificación recibida
        Console.WriteLine("Notificación recibida: " + payload.ToString());

        // Siempre devolver 200 OK para confirmar recepción
        return Ok();
    }
}
