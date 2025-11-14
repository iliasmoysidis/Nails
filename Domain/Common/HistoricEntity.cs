namespace Domain.Common;

public class HistoricEntity : BaseEntity
{
    public bool IsActive { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    protected HistoricEntity()
    {
        IsActive = true;
    }

    public void Delete()
    {
        IsActive = false;
        DeletedAt = DateTime.UtcNow;
    }
}