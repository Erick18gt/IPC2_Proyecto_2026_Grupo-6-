namespace OrbiNet.Models
{
    public class RelayRequest
    {
        public string SourceSatellite { get; set; } = string.Empty;
        public string DestinationSatellite { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
    }
}
