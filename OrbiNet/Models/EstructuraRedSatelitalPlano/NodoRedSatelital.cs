using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraBufferMensaje;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRedSatelitalPlano
{
    public class HeaderNode
    {
        public int Index { get; set; }
        public HeaderNode Next { get; set; }
        public MatrixNode Access { get; set; }

        public HeaderNode(int index)
        {
            Index = index;
            Next = null;
            Access = null;
        }
    }

    public class MatrixNode
    {
        public SatelitePlano Valor { get; set; }
        
        public MatrixNode Up { get; set; }
        public MatrixNode Down { get; set; }
        public MatrixNode Left { get; set; }
        public MatrixNode Right { get; set; }

        public ABBBufferMensaje Buffer { get; set; }

        public MatrixNode(SatelitePlano valor)
        {
            Valor = valor;
            Buffer = new ABBBufferMensaje();
            Up = Down = Left = Right = null;
        }
    }
}