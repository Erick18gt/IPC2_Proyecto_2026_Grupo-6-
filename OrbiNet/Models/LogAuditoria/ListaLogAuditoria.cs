namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.LogAuditoria
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
    }
}