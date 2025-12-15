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
        var date = dateTime.Date;
        var time = dateTime.TimeOfDay;

        if (_exceptions.TryGetValue(date, out var exception))
        {
            if (exception.IsDayOff) return false;

            return exception.TimeRanges.Any(r => r.Contains(time));
        }

        if (!_workingDays.TryGetValue(dateTime.DayOfWeek, out var workingDay)) return false;

        if (workingDay.IsDayOff) return false;

        return workingDay.TimeRanges.Any(r => r.Contains(time));
    }

    public bool IsOpenOn(DateOnly date)
    {
        if (_exceptions.TryGetValue(date, out var exception)) return !exception.IsDayOff;

        if (_workingDays.TryGetValue(date.DayOfWeek, out var workingDay)) return !workingDay.IsDayOff;

        return false;
    }

    public bool IsWithinStoreHours(DateOnly date, TimeRange range)
    {
        if (_exceptions.TryGetValue(date, out var exception))
        {
            if (exception.IsDayOff) return false;

            return exception.TimeRanges.Any(r => r.Start <= range.Start && r.End >= range.End);
        }

        if (!_workingDays.TryGetValue(date.DayOfWeek, out var workingDay)) return false;

        if (workingDay.IsDayOff) return false;

        return workingDay.TimeRanges.Any(r => r.Start <= range.Start && r.End >= range.End);
    }
}