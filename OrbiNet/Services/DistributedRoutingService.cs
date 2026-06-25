namespace OrbiNet.Services
{
    
    public class DistributedRoutingService
    {
        private readonly string[] nodos;
        private int cantidadNodos;

        public DistributedRoutingService()
        {
            nodos = new string[100];
            cantidadNodos = 0;
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
    }
}
