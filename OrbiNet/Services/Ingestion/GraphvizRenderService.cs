namespace OrbiNet.Services.Ingestion;

using System.Text;
using IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria;

public class GraphvizRenderService
{
    public string GenerarDotLogs(LogAuditoria[] logs)
    {
        StringBuilder dot = new StringBuilder();

        dot.AppendLine("digraph LogsAuditoria {");
        dot.AppendLine("rankdir=LR;");
        dot.AppendLine("node [shape=box];");

        if (logs.Length == 0)
        {
            dot.AppendLine("vacio [label=\"Sin logs\"];");
        }

        for (int i = 0; i < logs.Length; i++)
        {
            string descripcion = Escapar(logs[i].ObtenerDescripcion());
            dot.AppendLine($"log{i} [label=\"{descripcion}\"];");

            if (i < logs.Length - 1)
            {
                dot.AppendLine($"log{i} -> log{i + 1};");
            }
        }

        dot.AppendLine("}");
        return dot.ToString();
    }

    public string GenerarDotResultado(IngestionResult resultado)
    {
        StringBuilder dot = new StringBuilder();

        dot.AppendLine("digraph ResultadoIngestion {");
        dot.AppendLine("node [shape=box];");
        dot.AppendLine($"resultado [label=\"Success: {resultado.Success}\\nMensaje: {Escapar(resultado.Message)}\\nProcesados: {resultado.ProcessedNodes}\"];");
        dot.AppendLine("}");

        return dot.ToString();
    }

    public void GuardarDot(string contenidoDot, string rutaArchivo)
    {
        File.WriteAllText(rutaArchivo, contenidoDot);
    }

    private string Escapar(string texto)
    {
        return texto
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "");
    }
}