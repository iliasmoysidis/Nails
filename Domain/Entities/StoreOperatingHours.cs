using Domain.Common;

namespace Domain.Entities;

public class StoreOperatingHours : BaseEntity
{
    public int StoreId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan OpenTime { get; private set; }
    public TimeSpan CloseTime { get; private set; }
    public bool IsClosed { get; private set; } = false;
}
