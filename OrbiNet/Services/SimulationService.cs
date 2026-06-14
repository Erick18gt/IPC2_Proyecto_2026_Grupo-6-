namespace OrbiNet.Services
{
    /// <summary>
    /// Servicio encargado de administrar el avance de la simulación.
    /// </summary>
    public class SimulationService
    {
        private int tickActual;

        public SimulationService()
        {
            tickActual = 0;
        }

        /// <summary>
        /// Retorna el tick actual de la simulación.
        /// </summary>
        public int ObtenerTickActual()
        {
            return tickActual;
        }

        /// <summary>
        /// Avanza la simulación la cantidad de ticks indicada.
        /// </summary>
        public int AvanzarTicks(int cantidadTicks)
        {
            if (cantidadTicks < 0)
            {
                return tickActual;
            }

            tickActual += cantidadTicks;
            return tickActual;
        }

        public void Reiniciar()
        {
            tickActual = 0;
        }
    }
}
