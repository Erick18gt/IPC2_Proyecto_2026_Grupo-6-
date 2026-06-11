namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models.LogAuditoria
{
    public class NodoLogAuditoria
    {
        private LogAuditoria Valor { get; set; }
        private NodoLogAuditoria? Siguiente { get; set; }

        public NodoLogAuditoria(LogAuditoria log)
        {
            Valor = log;
            Siguiente = null;
        }
    }
}