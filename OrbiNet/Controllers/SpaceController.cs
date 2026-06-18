using Microsoft.AspNetCore.Mvc;
using OrbiNet.Services;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/space")]
    public class SpaceController : ControllerBase
    {
        private readonly SimulationService simulationService;

        public SpaceController(SimulationService simulationService)
        {
            this.simulationService = simulationService;
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
    }
}