using System;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria
{
    public class LogAuditoria
    {
        private DateTime Timestamp { get; set; }
        private string severity;
        private string message;

        public LogAuditoria(string severity, string message)
        {
            Timestamp = DateTime.Now;
            Severity = severity;
            Message = message;
        }

        public string Severity
        {
            get { return severity; }
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
            get { return message; } 
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
            return $"[{Timestamp}] - [{severity}] - {message}";
        }
    }
}