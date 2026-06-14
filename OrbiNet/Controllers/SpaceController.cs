using Microsoft.AspNetCore.Mvc;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/space")]
    public class SpaceController : ControllerBase
    {
        [HttpPost("config")]
        public IActionResult LoadConfiguration()
        {
            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Configuración recibida correctamente"
            });
        }

        [HttpPost("simulation/step")]
        public IActionResult AdvanceSimulation()
        {
            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Simulación ejecutada correctamente"
            });
        }
    }
}