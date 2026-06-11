namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.EstructuraLogAuditoria
{
    public class NodoLogAuditoria
    {
        public LogAuditoria Valor { get; set; }
        public NodoLogAuditoria? Siguiente { get; set; }

        public NodoLogAuditoria(LogAuditoria log)
        {
            Valor = log;
            Siguiente = null;
        }
    }
}