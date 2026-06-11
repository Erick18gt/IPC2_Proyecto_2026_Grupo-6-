using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.LogAuditoria_ListaEnlazadaSimple
{
    public class LogAuditoria
    {
        private DateTime timestamp;
        private string severity;
        private string message;

        public LogAuditoria(DateTime timestamp, string severity, string message)
        {
            this.timestamp = DateTime.Now;
            Severity = severity;
            Message = message;
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
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
            return $"[{timestamp}] - [{severity}] - {message}";
        }
    }
}