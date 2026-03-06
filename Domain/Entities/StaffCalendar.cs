using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class StaffCalendar
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private readonly Dictionary<DayOfWeek, WorkingDay> _workingDays = new();
    private readonly Dictionary<DateOnly, CalendarException> _exceptions = new();

    private StaffCalendar() { }

    private StaffCalendar(int storeId, int professionalId)
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
    }

    public static StaffCalendar Create(int storeId, int professionalId) => new(storeId, professionalId);

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

    public bool IsAvailable(UtcDateTime startAt, UtcDateTime endAt)
    {
        if (endAt <= startAt) return false;
        if (endAt.Date != startAt.Date) return false;

        var date = startAt.Date;
        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        return GetWorkingTimeRanges(date)
            .Any(r =>
                r.Start <= range.Start &&
                r.End >= range.End);
    }

    public void Clear()
    {
        _workingDays.Clear();
        _exceptions.Clear();
    }

    public bool TryGetWorkingDay(DayOfWeek day, out WorkingDay workingDay)
        => _workingDays.TryGetValue(day, out workingDay!);

    public bool TryGetException(DateOnly date, out CalendarException exception)
        => _exceptions.TryGetValue(date, out exception!);

    public IReadOnlyCollection<TimeRange> GetWorkingTimeRanges(DateOnly date)
    {
        if (_exceptions.TryGetValue(date, out var exception))
        {
            return exception.IsDayOff ? EmptyRanges : exception.TimeRanges;
        }

        if (_workingDays.TryGetValue(date.DayOfWeek, out var workingDay))
        {
            return workingDay.IsDayOff ? EmptyRanges : workingDay.TimeRanges;
        }

        return EmptyRanges;
    }

    public bool ConflictsWithWorkingDay(WorkingDay day)
    {
        if (day.IsDayOff) return false;

        if (!TryGetWorkingDay(day.Day, out var mine)) return false;

        if (mine.IsDayOff) return false;

        return TimeRange.AnyOverlap(mine.TimeRanges, day.TimeRanges);
    }

    public bool ConflictsWithException(CalendarException exception)
    {
        if (exception.IsDayOff) return false;

        var workingRanges = GetWorkingTimeRanges(exception.Date);

        if (!workingRanges.Any()) return false;

        return TimeRange.AnyOverlap(workingRanges, exception.TimeRanges);
    }

    public IReadOnlyCollection<CalendarException> GetExceptions()
        => _exceptions.Values.ToArray();

    private static readonly IReadOnlyCollection<TimeRange> EmptyRanges = Array.Empty<TimeRange>();
}