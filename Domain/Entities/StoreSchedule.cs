using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class StoreSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek Day { get; private set; }
    public TimeSpan? OpenTime { get; private set; }
    public TimeSpan? CloseTime { get; private set; }

    private StoreSchedule() { }

    public static StoreSchedule Create(int storeId, DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        ValidateScheduleInfo(openTime, closeTime);

        return new StoreSchedule
        {
            StoreId = storeId,
            Day = day,
            OpenTime = openTime,
            CloseTime = closeTime
        };
    }

    public static void ValidateScheduleInfo(TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        if ((openTime.HasValue && !closeTime.HasValue) || (!openTime.HasValue && closeTime.HasValue))
        {
            throw new DomainException("Both open and close times must be provided.");
        }

        if (openTime.HasValue && closeTime.HasValue && openTime >= closeTime)
        {
            throw new DomainException("Opening time must be earlier than closing time.");
        }
    }

    public bool IsFullDayClosed => !OpenTime.HasValue && !CloseTime.HasValue;
}
