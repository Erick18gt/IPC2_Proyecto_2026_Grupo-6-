namespace OrbiNet.Services
{
    public class DistributedRoutingService
    {
        private readonly string[] nodos;
        private int cantidadNodos;
        private readonly string[] historialMensajes;
        private int cantidadMensajes;

        public DistributedRoutingService()
        {
            nodos = new string[100];
            cantidadNodos = 0;
            historialMensajes = new string[100];
            cantidadMensajes = 0;
        }

        public bool RegistrarNodo(string idNodo)
        {
            if (cantidadNodos >= nodos.Length)
            {
                return false;
            }

            if (BuscarNodo(idNodo))
            {
                return false;
            }

            nodos[cantidadNodos] = idNodo;
            cantidadNodos++;

            return true;
        }

        public bool BuscarNodo(string idNodo)
        {
            for (int i = 0; i < cantidadNodos; i++)
            {
                if (nodos[i] == idNodo)
                {
                    return true;
                }
            }

            return false;
        }

        public int ObtenerCantidadNodos()
        {
            return cantidadNodos;
        }

        // Registra un mensaje en el historial
        public void RegistrarMensaje(string origen, string destino, string mensaje)
        {
            if (cantidadMensajes >= historialMensajes.Length)
            {
                return;
            }

            historialMensajes[cantidadMensajes] =
                $"{origen} -> {destino}: {mensaje}";

            cantidadMensajes++;
        }

        // Devuelve el historial de mensajes
        public string[] ObtenerHistorial()
        {
            string[] historial = new string[cantidadMensajes];

            for (int i = 0; i < cantidadMensajes; i++)
            {
                historial[i] = historialMensajes[i];
            }

            return historial;
        }
    }
}