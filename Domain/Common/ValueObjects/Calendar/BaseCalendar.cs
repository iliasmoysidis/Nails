namespace Domain.Common.ValueObjects.Calendar;

public abstract class BaseCalendar
{
    protected readonly List<WorkingDay> _workingDays = new();
    public IReadOnlyCollection<WorkingDay> WorkingDays => _workingDays.AsReadOnly();
    
    protected readonly List<CalendarException> _exceptions = new();
    public IReadOnlyCollection<CalendarException> Exceptions => _exceptions.AsReadOnly();

    protected static readonly IReadOnlyCollection<TimeRange> EmptyRanges
        = Array.Empty<TimeRange>();

    protected void SetWorkingDay(WorkingDay day)
    {
        _workingDays.RemoveAll(d => d.Day == day.Day);
        _workingDays.Add(day);
    }

    protected void SetException(CalendarException exception)
    {
        _exceptions.RemoveAll(e => e.Date == exception.Date);
        _exceptions.Add(exception);
    }

    protected void SetDayOff(DayOfWeek day)
        => SetWorkingDay(WorkingDay.DayOff(day));

    protected void RemoveException(DateOnly date)
        => _exceptions.RemoveAll(e => e.Date == date);

    protected IReadOnlyCollection<TimeRange> ResolveTimeRanges(DateOnly date)
    {
        var exception = _exceptions.FirstOrDefault(e => e.Date == date);
        if (exception is not null)
            return exception.IsDayOff ? EmptyRanges : exception.TimeRanges;

        var workingDay = _workingDays.FirstOrDefault(d => d.Day == date.DayOfWeek);
        if (workingDay is not null)
            return workingDay.IsDayOff ? EmptyRanges : workingDay.TimeRanges;

        return EmptyRanges;
    }
}
