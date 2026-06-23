namespace OrbiNet.Services.Ingestion;

public class TransactionScope
{
    public bool IsActive { get; private set; }
    public bool IsCommitted { get; private set; }
    public bool IsRolledBack { get; private set; }
    public string LastError { get; private set; } = "";

    public void Begin()
    {
        IsActive = true;
        IsCommitted = false;
        IsRolledBack = false;
        LastError = "";
    }

    public void Commit()
    {
        IsCommitted = true;
        IsRolledBack = false;
        IsActive = false;
    }

    public void Rollback(string error)
    {
        IsRolledBack = true;
        IsCommitted = false;
        IsActive = false;
        LastError = error;
    }

    public string ObtenerEstado()
    {
        if (IsCommitted) return "COMMIT";
        if (IsRolledBack) return "ROLLBACK";
        if (IsActive) return "ACTIVE";
        return "IDLE";
    }
}