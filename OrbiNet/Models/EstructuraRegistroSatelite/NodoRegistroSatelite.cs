namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRegistroSatelite
{
    public class NodoRegistroSatelite
    {
        public RegistroSatelite Valor { get; set; }
        public int Height { get; set; }
        public NodoRegistroSatelite LeftChild { get; set; }
        public NodoRegistroSatelite RightChild { get; set; }

        public NodoRegistroSatelite(RegistroSatelite valor)
        {
            Valor = valor;
            Height = 1;
            LeftChild = null;
            RightChild = null;
        }
    }
}