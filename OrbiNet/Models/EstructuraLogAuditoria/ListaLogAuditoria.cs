namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria
{
    public class ListaLogAuditoria : IAbstractCollection
    {
        private NodoLogAuditoria? cabeza;
        private NodoLogAuditoria? cola;
        private int tamano;

        public ListaLogAuditoria()
        {
            cabeza = null;
            cola = null;
            tamano = 0;
        }

        public int Count => tamano;

        public bool IsEmpty => cabeza == null;

        public void Clear()
        {
            cabeza = null;
            cola = null;
            tamano = 0;
        }

        public void InsertarLog(LogAuditoria log)
        {
            NodoLogAuditoria nuevoNodo = new NodoLogAuditoria(log);
            if (IsEmpty)
            {
                cabeza = nuevoNodo;
                cola = nuevoNodo;
            }
            else
            {
                cola.Siguiente = nuevoNodo;
                cola = nuevoNodo;
            }
            tamano++;
        }

        public LogAuditoria[] Recorrer(){
            LogAuditoria[] registros = new LogAuditoria[tamano];
            NodoLogAuditoria? actual = cabeza;
            int posicion = 0;
            while (actual != null) {
                registros[posicion] = actual.Valor;
                actual = actual.Siguiente;
                posicion++;
            }
            return registros;
        }

        public void mostrarLog(){
            LogAuditoria[] registros = Recorrer();
            for (int i = 0; i < registros.Length; i++) {
                Console.WriteLine(registros[i].ObtenerDescripcion());
            }
        }
    }
}