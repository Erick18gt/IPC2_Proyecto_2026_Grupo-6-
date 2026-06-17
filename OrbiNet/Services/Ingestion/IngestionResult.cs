namespace OrbiNet.Services.Ingestion;

public class IngestionResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public int ProcessedNodes { get; set; }
}