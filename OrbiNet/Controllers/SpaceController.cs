using Microsoft.AspNetCore.Mvc;
using OrbiNet.Models;
using OrbiNet.Services;
using OrbiNet.Services.Ingestion;

namespace OrbiNet.Controllers
{
    [ApiController]
    [Route("api/space")]
    public class SpaceController : ControllerBase
    {
        private readonly SimulationService simulationService;
        private readonly DistributedRoutingService routingService;
        private readonly XmlIngestionService xmlIngestionService;

        public SpaceController(
            SimulationService simulationService,
            DistributedRoutingService routingService,
            XmlIngestionService xmlIngestionService)
        {
            this.simulationService = simulationService;
            this.routingService = routingService;
            this.xmlIngestionService = xmlIngestionService;
        }

        [HttpPost("config")]
        public IActionResult LoadConfiguration([FromBody] ConfigRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.XmlContent))
            {
                return BadRequest(new
                {
                    Estado = "Error",
                    Mensaje = "El XML está vacío"
                });
            }

            IngestionResult resultado = xmlIngestionService.CargarXml(request.XmlContent);

            if (!resultado.Success)
            {
                return BadRequest(new
                {
                    Estado = "Rollback",
                    Mensaje = resultado.Message,
                    Procesados = resultado.ProcessedNodes,
                    Transaccion = xmlIngestionService.ObtenerEstadoTransaccion(),
                    LogsDot = xmlIngestionService.GenerarDotLogs()
                });
            }

            return Ok(new
            {
                Estado = "Commit",
                Mensaje = resultado.Message,
                Procesados = resultado.ProcessedNodes,
                Transaccion = xmlIngestionService.ObtenerEstadoTransaccion(),
                TablaRed = xmlIngestionService.GenerarTablaRedSatelital(),
                LogsDot = xmlIngestionService.GenerarDotLogs(),
                ResultadoDot = xmlIngestionService.GenerarDotResultado(resultado)
            });
        }

        [HttpGet("logs")]
        public IActionResult ObtenerLogs()
        {
            return Ok(xmlIngestionService.ObtenerLogs());
        }

        [HttpGet("graph/logs")]
        public IActionResult ObtenerGraphvizLogs()
        {
            return Ok(new
            {
                Dot = xmlIngestionService.GenerarDotLogs()
            });
        }

        [HttpGet("transaction/status")]
        public IActionResult EstadoTransaccion()
        {
            return Ok(new
            {
                Estado = xmlIngestionService.ObtenerEstadoTransaccion()
            });
        }

        [HttpGet("network/table")]
        public IActionResult ObtenerTablaRed()
        {
            return Ok(new
            {
                Tabla = xmlIngestionService.GenerarTablaRedSatelital()
            });
        }

        [HttpGet("state")]
        public IActionResult GetState()
        {
            return Ok(new
            {
                TickActual = simulationService.ObtenerTickActual(),
                EstadoTransaccion = xmlIngestionService.ObtenerEstadoTransaccion(),
                CantidadNodos = routingService.ObtenerCantidadNodos()
            });
        }

        [HttpGet("topology")]
        public IActionResult ObtenerTopologia()
        {
            return Ok(new
            {
                Topologia = xmlIngestionService.GenerarTablaRedSatelital()
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