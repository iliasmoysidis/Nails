using Domain.Common;

namespace Domain.Entities;

public class StoreOperatingHours : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan OpenTime { get; private set; }
    public TimeSpan CloseTime { get; private set; }
    public bool IsClosed { get; private set; }

    public Store Store { get; private set; } = null!;

    private StoreOperatingHours()
    {
        IsClosed = false;
    }
}
