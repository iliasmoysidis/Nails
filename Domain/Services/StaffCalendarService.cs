using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public sealed class StaffCalendarService
{
    public void SetWorkingDay(StaffCalendar calendar, IReadOnlyCollection<StaffCalendar> otherCalendars, Staff staff, int ownerId, WorkingDay workingDay)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(calendar.ProfessionalId);

        if (otherCalendars.Any(c => c.ConflictsWithRecurring(workingDay)))
            throw new DomainException($"Professional has a conflicting recurring schedule on {workingDay.Day}");

        calendar.SetWorkingDay(workingDay);
    }

    public void SetDayOff(StaffCalendar calendar, Staff staff, int ownerId, DayOfWeek day)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(calendar.ProfessionalId);
        calendar.SetDayOff(day);
    }

    public void AddException(StaffCalendar calendar, IReadOnlyCollection<StaffCalendar> otherCalendars, Staff staff, int ownerId, CalendarException exception)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(calendar.ProfessionalId);

        if (otherCalendars.Any(c => c.ConflictsWithDateSpecific(exception)))
            throw new DomainException($"Professional has a conflicting schedule on {exception.Date}");

        calendar.AddException(exception);
    }

    public void RemoveException(StaffCalendar calendar, Staff staff, int ownerId, DateOnly date)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(calendar.ProfessionalId);
        calendar.RemoveException(date);
    }
}