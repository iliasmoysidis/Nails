using Domain.Entities;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public sealed class StoreCalendarService
{
    public void SetWorkingDay(StoreCalendar calendar, Staff staff, int ownerId, WorkingDay workingDay)
    {
        staff.EnsureOwner(ownerId);
        calendar.SetWorkingDay(workingDay);
    }

    public void SetDayOff(StoreCalendar calendar, Staff staff, int ownerId, DayOfWeek day)
    {
        staff.EnsureOwner(ownerId);
        calendar.SetDayOff(day);
    }

    public void AddException(StoreCalendar calendar, Staff staff, int ownerId, CalendarException exception)
    {
        staff.EnsureOwner(ownerId);
        calendar.AddException(exception);
    }

    public void RemoveException(StoreCalendar calendar, Staff staff, int ownerId, DateOnly date)
    {
        staff.EnsureOwner(ownerId);
        calendar.RemoveException(date);
    }
}