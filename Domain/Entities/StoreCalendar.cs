using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class StoreCalendar : BaseCalendar
{
    public int StoreId { get; }

    private StoreCalendar() { }

    private StoreCalendar(int storeId)
    {
        StoreId = storeId;
    }

    public static StoreCalendar Create(int storeId)
        => new(storeId);


    public bool IsOpenAt(UtcDateTime dateTime)
    {
        var ranges = ResolveTimeRanges(dateTime.Date);
        var time = dateTime.TimeOfDay;

        return ranges.Any(r => r.Contains(time));
    }

    public bool IsOpenOn(DateOnly date)
        => ResolveTimeRanges(date).Any();

    public bool IsWithinStoreHours(DateOnly date, TimeRange range)
        => ResolveTimeRanges(date)
            .Any(r => r.Start <= range.Start && r.End >= range.End);

    public bool IsWithinWeeklyStoreHours(DayOfWeek day, TimeRange range)
    {
        if (!_workingDays.TryGetValue(day, out var workingDay))
            return false;

        if (workingDay.IsDayOff)
            return false;

        return workingDay.TimeRanges.Any(r =>
            r.Start <= range.Start &&
            r.End >= range.End);
    }
}