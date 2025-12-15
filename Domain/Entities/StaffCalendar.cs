using Domain.ValueObjects.Calendar;
using Domain.Exceptions;

namespace Domain.Entities;

public class StaffCalendar
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private readonly Dictionary<DayOfWeek, WorkingDay> _workingDays = new();
    private readonly Dictionary<DateOnly, CalendarException> _exceptions = new();

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

    public bool IsProfessionalAvailable(DateTime startAt, DateTime endAt)
    {
        if (endAt <= startAt) throw new DomainException("End time must be after start time.");

        var date = DateOnly.FromDateTime(startAt);

        if (DateOnly.FromDateTime(endAt) != date) return false;

        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        if (_exceptions.TryGetValue(date, out var exception))
        {
            if (exception.IsDayOff) return false;

            return exception.TimeRanges.Any(r => r.Start <= range.Start && r.End >= range.End);
        }

        if (!_workingDays.TryGetValue(startAt.DayOfWeek, out var workingDay)) return false;

        if (workingDay.IsDayOff) return false;

        return workingDay.TimeRanges.Any(r => r.Start <= range.Start && r.End >= range.End);
    }

    public bool TryGetWorkingDay(DayOfWeek day, out WorkingDay workingDay)
        => _workingDays.TryGetValue(day, out workingDay!);

    public bool TryGetException(DateOnly date, out CalendarException exception)
        => _exceptions.TryGetValue(date, out exception!);
}