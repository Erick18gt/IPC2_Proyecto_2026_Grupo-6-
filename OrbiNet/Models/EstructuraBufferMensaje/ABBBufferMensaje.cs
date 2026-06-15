namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraBufferMensaje
{
    public class ABBBufferMensaje : IAbstractCollection
    {
        private NodoBufferMensaje raiz;
        private int tamano;

        public int Count => tamano;
        public bool IsEmpty => raiz == null;

        public void Clear() 
        { 
            raiz = null; 
            tamano = 0; 
        }

        public void Enqueue(BufferMensaje mensaje)
        {
            raiz = EnqueueRecursivo(raiz, mensaje);
            tamano++;
        }

        private NodoBufferMensaje EnqueueRecursivo(NodoBufferMensaje nodoActual, BufferMensaje mensaje)
        {
            if (nodoActual == null)
            {
                return new NodoBufferMensaje(mensaje);
            }
            
            if(mensaje.Priority <= nodoActual.Valor.Priority)
            {
                nodoActual.Izquierdo = EnqueueRecursivo(nodoActual.Izquierdo, mensaje);
            }
            else
            {
                nodoActual.Derecho = EnqueueRecursivo(nodoActual.Derecho, mensaje);
            }
            return nodoActual;
        }

        public BufferMensaje Dequeue()
        {
            if(raiz == null)
            {
                return null;
            }

            NodoBufferMensaje padre = null;
            NodoBufferMensaje actual = raiz;

            while (actual.Derecho != null)
            {
                padre = actual;
                actual = actual.Derecho;
            }

            BufferMensaje mensaje = actual.Valor;

            if(padre != null)
            {
                padre.Derecho = actual.Izquierdo;
            }
            else
            {
                raiz = actual.Izquierdo;
            }

            tamano--;
            return mensaje;
        }

        public void MostrarArbolVisual()
        {
            Console.WriteLine("\n=== ESTADO DEL ÁRBOL BÚSQUEDA BINARIA ===");
            if (raiz == null)
            {
                Console.WriteLine("El buffer está vacío.");
            }
            else
            {
            ImprimirNodo(raiz, "", true, true);
            }
            Console.WriteLine("=========================================\n");
        }

        private void ImprimirNodo(NodoBufferMensaje nodoActual, string prefijo, bool esIzquierdo, bool esRaiz)
        {
            if (nodoActual != null)
            {
                // Mostrar el nodo actual
                Console.Write(prefijo);
                Console.Write(esRaiz ? "├── " : (esIzquierdo ? "├── " : "└── "));
                Console.WriteLine($"[{nodoActual.Valor.Priority}] {nodoActual.Valor.Content}");

                // Preparar prefijos para los hijos
                string prefijoHijo = prefijo + (esRaiz ? "│   " : (esIzquierdo ? "│   " : "    "));

                // Mostrar hijos (primero el derecho para simular vista "de lado")
                if (nodoActual.Derecho != null)
                {
                    ImprimirNodo(nodoActual.Derecho, prefijoHijo, false, false);
                }
                if (nodoActual.Izquierdo != null)
                {
                    ImprimirNodo(nodoActual.Izquierdo, prefijoHijo, true, false);
                }
            }
        }
    }
}