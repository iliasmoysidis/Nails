using Domain.Common.Exceptions;
using Domain.Common.ValueObjects.Calendar;
using Domain.Common.ValueObjects;
using Domain.Schedule.Entities;

namespace Domain.Schedule;

public class ProfessionalSchedule
{
    public int ProfessionalId { get; }
    private readonly Dictionary<int, StaffCalendar> _calendars = [];
    public IReadOnlyCollection<StaffCalendar> Calendars => _calendars.Values;

    private ProfessionalSchedule() { }

    private ProfessionalSchedule(int professionalId)
    {
        ProfessionalId = professionalId;
    }

    public void AddCalendar(StaffCalendar calendar)
    {
        EnsureCalendarBelongsToProfessional(calendar);

        if (_calendars.ContainsKey(calendar.StoreId))
            throw new InvariantException("Professional already has a calendar for this store.");

        EnsureCalendarDoesNotConflict(calendar);

        _calendars.Add(calendar.StoreId, calendar);
    }

    public void RemoveCalendar(int storeId)
    {
        if (!_calendars.Remove(storeId))
            throw new NotFoundException("Calendar not found.");
    }

    internal StaffCalendar GetCalendar(int storeId)
    {
        var calendar = _calendars.GetValueOrDefault(storeId)
            ?? throw new NotFoundException("Calendar not found.");

        return calendar;
    }

    public bool IsWorkingAtStore(int storeId, UtcDateTime startAt, UtcDateTime endAt)
    {
        var calendar = GetCalendar(storeId);

        return calendar.IsAvailable(startAt, endAt);
    }

    public void EnsureWorkingDayDoesNotConflict(int storeId, WorkingDay workingDay)
    {
        if (workingDay.IsDayOff)
            return;

        foreach (var calendar in _calendars.Values)
        {
            if (calendar.StoreId == storeId)
                continue;

            if (calendar.ConflictsWithWorkingDay(workingDay))
            {
                throw new InvariantException("Professional already works for another store during this time.");
            }
        }
    }

    public void EnsureExceptionDoesNotConflict(int storeId, CalendarException exception)
    {
        if (exception.IsDayOff)
            return;

        foreach (var calendar in _calendars.Values)
        {
            if (calendar.StoreId == storeId)
                continue;

            if (calendar.ConflictsWithException(exception))
                throw new InvariantException("Professional already works for another store during this time.");
        }
    }

    public static ProfessionalSchedule Rehydrate(int professionalId, IEnumerable<StaffCalendar> calendars)
    {
        var schedule = new ProfessionalSchedule(professionalId);

        foreach (var calendar in calendars)
        {
            schedule.AddCalendar(calendar);
        }

        return schedule;
    }

    private void EnsureCalendarBelongsToProfessional(StaffCalendar calendar)
    {
        if (calendar.ProfessionalId != ProfessionalId)
            throw new InvariantException("Calendar does not belong to this professional.");
    }

    private void EnsureCalendarDoesNotConflict(StaffCalendar calendar)
    {
        foreach (var workingDay in calendar.GetWorkingDays())
        {
            EnsureWorkingDayDoesNotConflict(calendar.StoreId, workingDay);
        }

        foreach (var exception in calendar.GetExceptions())
        {
            EnsureExceptionDoesNotConflict(calendar.StoreId, exception);
        }
    }
}
