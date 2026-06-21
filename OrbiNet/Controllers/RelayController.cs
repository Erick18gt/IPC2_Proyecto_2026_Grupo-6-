using Microsoft.AspNetCore.Mvc;
using OrbiNet.Models;
using OrbiNet.Services;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/relay")]
    public class RelayController : ControllerBase
    {
        private readonly DistributedRoutingService routingService;

        public RelayController(
            DistributedRoutingService routingService)
        {
            this.routingService = routingService;
        }

        [HttpPost("send")]
        public IActionResult SendMessage(
            [FromBody] MessageRequest request)
        {
            if (request == null)
            {
                return BadRequest(new
                {
                    Estado = "Error",
                    Mensaje = "Solicitud inválida"
                });
            }

            if (string.IsNullOrWhiteSpace(request.Mensaje))
            {
                return BadRequest(new
                {
                    Estado = "Error",
                    Mensaje = "El mensaje está vacío"
                });
            }

            if (!routingService.BuscarNodo(request.Origen))
            {
                return NotFound(new
                {
                    Estado = "Error",
                    Mensaje = "Nodo origen no encontrado"
                });
            }

            if (!routingService.BuscarNodo(request.Destino))
            {
                return NotFound(new
                {
                    Estado = "Error",
                    Mensaje = "Nodo destino no encontrado"
                });
            }

            return Ok(new
            {
                Estado = "Encolado",
                Mensaje = "Mensaje registrado para transmisión",
                Origen = request.Origen,
                Destino = request.Destino
            });
        }
    }
}