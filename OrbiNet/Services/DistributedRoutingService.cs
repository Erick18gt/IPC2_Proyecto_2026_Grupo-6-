namespace OrbiNet.Services
{
    /// <summary>
    /// Servicio encargado de administrar los nodos de la red.
    /// </summary>
    public class DistributedRoutingService
    {
        private readonly string[] nodos;
        private int cantidadNodos;

        public DistributedRoutingService()
        {
            nodos = new string[100];
            cantidadNodos = 0;
        }

        /// <summary>
        /// Registra un nodo en la red.
        /// </summary>
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

        /// <summary>
        /// Busca un nodo por su identificador.
        /// </summary>
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

        /// <summary>
        /// Retorna la cantidad de nodos registrados.
        /// </summary>
        public int ObtenerCantidadNodos()
        {
            return cantidadNodos;
        }
    }
}
