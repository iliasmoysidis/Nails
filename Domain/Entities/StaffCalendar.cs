using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class StaffCalendar : BaseCalendar
{
    public int StoreId { get; }
    public int ProfessionalId { get; }

    private StaffCalendar() { }

    public StaffCalendar(int storeId, int professionalId)
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
    }

    public void AddVacation(DateOnly date)
    {
        SetException(CalendarException.DayOff(date));
    }

    public void SetSpecialAvailability(CalendarException exception)
    {
        SetException(exception);
    }

    public void RemoveSpecialAvailability(DateOnly date)
    {
        RemoveException(date);
    }

    public void SetWorkingHours(WorkingDay workingDay)
    {
        SetWorkingDay(workingDay);
    }

    public void RestDay(DayOfWeek day)
    {
        SetDayOff(day);
    }

    public bool IsAvailable(UtcDateTime startAt, UtcDateTime endAt)
    {
        if (endAt <= startAt || startAt.Date != endAt.Date)
            return false;

        var range = new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay);

        return ResolveTimeRanges(startAt.Date)
            .Any(r => r.Start <= range.Start && r.End >= range.End);
    }

    public bool ConflictsWithWorkingDay(WorkingDay day)
    {
        if (day.IsDayOff)
            return false;

        if (!_workingDays.TryGetValue(day.Day, out var existing) || existing.IsDayOff)
            return false;

        return TimeRange.AnyOverlap(existing.TimeRanges, day.TimeRanges);
    }

    public bool ConflictsWithException(CalendarException exception)
    {
        if (exception.IsDayOff)
            return false;

        var workingRanges = ResolveTimeRanges(exception.Date);

        return workingRanges.Any()
            && TimeRange.AnyOverlap(workingRanges, exception.TimeRanges);
    }

    public static StaffCalendar Rehydrate(
        int storeId,
        int professionalId,
        IEnumerable<WorkingDay> workingDays,
        IEnumerable<CalendarException> exceptions
    )
    {
        var calendar = new StaffCalendar(storeId, professionalId);

        foreach (var day in workingDays)
            calendar.SetWorkingDay(day);

        foreach (var ex in exceptions)
            calendar.SetException(ex);

        return calendar;
    }
}
