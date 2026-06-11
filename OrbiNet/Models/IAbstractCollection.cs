namespace IPC2_Proyecto_2026_Grupo_6_.OrbiNet.Models
{
    public interface IAbstractCollection
    {
        int Count { get; }
        void Clear();
        bool IsEmpty { get; }
    }
}