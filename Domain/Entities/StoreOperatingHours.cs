using Domain.Common;

namespace Domain.Entities;

public class StoreOperatingHours : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeSpan OpenTime { get; private set; }
    public TimeSpan CloseTime { get; private set; }
    public bool IsClosed { get; private set; }

    public Store Store { get; private set; } = null!;

    private StoreOperatingHours()
    {
        IsClosed = false;
    }

    public static StoreOperatingHours Create(int storeId, DayOfWeek day, TimeSpan openTime, TimeSpan closeTime)
    {
        return new StoreOperatingHours
        {
            StoreId = storeId,
            Day = day,
            OpenTime = openTime,
            CloseTime = closeTime
        };
    }
}
