using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class StoreCalendar
{
    public int StoreId { get; }

    private readonly Dictionary<DayOfWeek, WorkingDay> _workingDays = new();
    private readonly Dictionary<DateOnly, CalendarException> _exceptions = new();

    private StoreCalendar(int storeId)
    {
        StoreId = storeId;
    }

    public static StoreCalendar Create(int storeId) => new(storeId);

    public void SetWorkingDay(WorkingDay day)
    {
        _workingDays[day.Day] = day;
    }

    public void SetDayOff(DayOfWeek day)
    {
        _workingDays[day] = WorkingDay.DayOff(day);
    }

    public void AddException(CalendarException exception)
    {
        _exceptions[exception.Date] = exception;
    }

    public void RemoveException(DateOnly date)
    {
        _exceptions.Remove(date);
    }

    public bool IsOpenAt(UtcDateTime dateTime)
    {
        var ranges = GetWorkingTimeRanges(dateTime.Date);
        var time = dateTime.TimeOfDay;

        return ranges.Any(r => r.Contains(time));
    }

    public bool IsOpenOn(DateOnly date)
    {
        return GetWorkingTimeRanges(date).Any();
    }

    public bool IsWithinStoreHours(DateOnly date, TimeRange range)
    {
        var ranges = GetWorkingTimeRanges(date);
        return ranges.Any(r => r.Start <= range.Start && r.End >= range.End);
    }

    public IReadOnlyCollection<TimeRange> GetWorkingTimeRanges(DateOnly date)
    {
        if (_exceptions.TryGetValue(date, out var exception))
        {
            return exception.IsDayOff ? Array.Empty<TimeRange>() : exception.TimeRanges;
        }

        if (_workingDays.TryGetValue(date.DayOfWeek, out var workingDay))
        {
            return workingDay.IsDayOff ? Array.Empty<TimeRange>() : workingDay.TimeRanges;
        }

        return Array.Empty<TimeRange>();
    }

    public IReadOnlyCollection<CalendarException> GetExceptions()
        => _exceptions.Values.ToList().AsReadOnly();
}