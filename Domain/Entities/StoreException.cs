using Domain.Common;

namespace Domain.Entities;

public class StoreException : BaseEntity
{
    public int StoreId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan CloseTime { get; private set; }
    public string? Reason { get; private set; }
    public bool IsClosed { get; private set; } = true;
}
