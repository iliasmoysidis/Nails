using Domain.Common;

namespace Domain.Entities;

public class StoreException
{
    public int Id { get; set; }
    public int StoreId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan? OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public string? Reason { get; set; }
    public bool IsClosed { get; set; } = true;

    public Store Store { get; set; } = null!;
}
