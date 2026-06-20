namespace Domain.Common.ValueObjects.Calendar;

public abstract class BaseCalendar
{
    protected readonly Dictionary<DayOfWeek, WorkingDay> _workingDays = new();
    protected readonly Dictionary<DateOnly, CalendarException> _exceptions = new();

    protected static readonly IReadOnlyCollection<TimeRange> EmptyRanges
        = Array.Empty<TimeRange>();

    protected void SetWorkingDay(WorkingDay day)
    {
        _workingDays[day.Day] = day;
    }

    protected void SetException(CalendarException exception)
        => _exceptions[exception.Date] = exception;

    protected void SetDayOff(DayOfWeek day)
        => _workingDays[day] = WorkingDay.DayOff(day);

    protected void RemoveException(DateOnly date)
        => _exceptions.Remove(date);

    public IReadOnlyCollection<WorkingDay> GetWorkingDays()
        => _workingDays.Values;

    public IReadOnlyCollection<CalendarException> GetExceptions()
        => _exceptions.Values;

    protected IReadOnlyCollection<TimeRange> ResolveTimeRanges(DateOnly date)
    {
        if (_exceptions.TryGetValue(date, out var exception))
            return exception.IsDayOff ? EmptyRanges : exception.TimeRanges;

        if (_workingDays.TryGetValue(date.DayOfWeek, out var workingDay))
            return workingDay.IsDayOff ? EmptyRanges : workingDay.TimeRanges;

        return EmptyRanges;
    }
}
