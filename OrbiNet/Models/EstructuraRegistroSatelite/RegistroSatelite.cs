namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraRegistroSatelite
{
    public class RegistroSatelite
    {
        public string SatelliteId { get; set; }
        public string Name { get; set; }
        public double Frequency { get; set; }
        
        public RegistroSatelite(string satelliteId, string name, double frequency)
        {
            SatelliteId = satelliteId;
            Name = name;
            Frequency = frequency;
        }
    }
}