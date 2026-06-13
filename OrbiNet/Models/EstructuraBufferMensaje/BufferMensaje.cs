using System;

namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraBufferMensaje
{
    public class BufferMensaje
    {
        public string HexCode { get; set; }
        public string EmisorId { get; set; }
        public string DestIp { get; set; }
        public int Priority { get; set; }
        public string Content { get; set; }

        public BufferMensaje(string hexCode, string emisorId, string destIp, int priority, string content)
        {
            HexCode = hexCode;
            EmisorId = emisorId;
            DestIp = destIp;
            Priority = priority;
            Content = content;
        }

        public string ObtenerDescripcion()
        {
            return $"[{HexCode}] - [{EmisorId}] - [{DestIp}] - [{Priority}] - {Content}";
        }
    }
}