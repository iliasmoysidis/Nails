using Domain.Common.ValueObjects.Calendar;
using Domain.Common.ValueObjects;

namespace Domain.Calendar;

public class StoreCalendar : BaseCalendar
{
    public int StoreId { get; }

    private StoreCalendar() { }

    public StoreCalendar(int storeId)
    {
        StoreId = storeId;
    }

    public void AddHoliday(DateOnly date)
    {
        SetException(CalendarException.DayOff(date));
    }

    public void SetSpecialOpeningHours(DateOnly date, IEnumerable<TimeRange> ranges)
    {
        SetException(CalendarException.PartialDay(date, ranges));
    }

    public void RemoveSpecialOpeningHours(DateOnly date)
    {
        RemoveException(date);
    }

    public void SetOpeningHours(DayOfWeek day, IEnumerable<TimeRange> ranges)
    {
        SetWorkingDay(WorkingDay.WithRanges(day, ranges));
    }

    public void CloseDay(DayOfWeek day)
    {
        SetDayOff(day);
    }

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

    public static StoreCalendar Rehydrate(
        int storeId,
        IEnumerable<WorkingDay> workingDays,
        IEnumerable<CalendarException> exceptions
    )
    {
        var calendar = new StoreCalendar(storeId);

        foreach (var day in workingDays)
            calendar.SetWorkingDay(day);

        foreach (var exception in exceptions)
            calendar.SetException(exception);

        return calendar;
    }
}
