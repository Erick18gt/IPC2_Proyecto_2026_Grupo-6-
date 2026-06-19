using Microsoft.AspNetCore.Mvc;
using OrbiNet.Services;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/space")]
    public class SpaceController : ControllerBase
    {
        private readonly SimulationService simulationService;
        private readonly DistributedRoutingService routingService;

        public SpaceController(
            SimulationService simulationService,
            DistributedRoutingService routingService)
        {
            this.simulationService = simulationService;
            this.routingService = routingService;
        }

        [HttpPost("config")]
        public IActionResult LoadConfiguration()
        {
            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Configuración recibida correctamente"
            });
        }

        [HttpGet("simulation/status")]
        public IActionResult GetSimulationStatus()
        {
            return Ok(new
            {
                TickActual = simulationService.ObtenerTickActual()
            });
        }

        [HttpPost("simulation/step")]
        public IActionResult AdvanceSimulation()
        {
            int nuevoTick = simulationService.AvanzarTicks(1);

            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Simulación ejecutada correctamente",
                TickActual = nuevoTick
            });
        }

        [HttpPost("simulation/reset")]
        public IActionResult ResetSimulation()
        {
            simulationService.Reiniciar();

            return Ok(new
            {
                Estado = "Exitoso",
                Mensaje = "Simulación reiniciada",
                TickActual = simulationService.ObtenerTickActual()
            });
        }

        [HttpPost("node/register/{idNodo}")]
        public IActionResult RegisterNode(string idNodo)
        {
            bool registrado = routingService.RegistrarNodo(idNodo);

            return Ok(new
            {
                Estado = registrado ? "Exitoso" : "Error",
                Nodo = idNodo,
                Registrado = registrado
            });
        }

        [HttpGet("node/count")]
        public IActionResult GetNodeCount()
        {
            return Ok(new
            {
                CantidadNodos = routingService.ObtenerCantidadNodos()
            });
        }

        [HttpGet("node/exists/{idNodo}")]
        public IActionResult NodeExists(string idNodo)
        {
            return Ok(new
            {
                Nodo = idNodo,
                Existe = routingService.BuscarNodo(idNodo)
            });
        }
    }
}