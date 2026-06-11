using System;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.LogAuditoria
{
    public class LogAuditoria
    {
        public DateTime Timestamp { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }

        public LogAuditoria(string severity, string message)
        {
            Timestamp = DateTime.Now;
            Severity = severity;
            Message = message;
        }

        public string Severity
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("La severidad no puede estar vacía.");
                }
                severity = value;
            }
        }

        public string Message
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("El mensaje no puede estar vacío.");
                }
                message = value;
            }
        }

        public string ObtenerDescripcion()
        {
            return $"[{timestamp}] - [{severity}] - {message}";
        }
    }
}