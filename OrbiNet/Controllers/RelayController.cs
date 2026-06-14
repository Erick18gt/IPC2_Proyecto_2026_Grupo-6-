using Microsoft.AspNetCore.Mvc;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/relay")]
    public class RelayController : ControllerBase
    {
        [HttpPost("send")]
        public IActionResult SendMessage()
        {
            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Mensaje procesado correctamente"
            });
        }
    }
}
