using Domain.Common;

namespace Domain.Entities;

public class StoreException : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan CloseTime { get; private set; }
    public string? Reason { get; private set; }
    public bool IsClosed { get; private set; }

    public Store Store { get; private set; } = null!;

    private StoreException()
    {
        IsClosed = true;
    }
}
