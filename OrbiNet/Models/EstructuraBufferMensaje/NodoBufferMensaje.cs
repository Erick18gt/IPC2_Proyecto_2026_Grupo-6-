namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraBufferMensaje
{
    public class NodoBufferMensaje
    {
        public BufferMensaje Valor {get; set;}
        public NodoBufferMensaje Izquierdo {get; set;}
        public NodoBufferMensaje Derecho {get; set;}

        public NodoBufferMensaje(BufferMensaje valor)
        {
            Valor = valor;
            Izquierdo = null;
            Derecho = null;
        }
    }
}