namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRedSatelitalPlano
{
    public class SatelitePlano
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Id { get; set; }
        public string IpAddress { get; set; }

        public SatelitePlano(int row, int col, string id, string ipAddress)
        {
            Row = row;
            Col = col;
            Id = id;
            IpAddress = ipAddress;
        }
    }
}