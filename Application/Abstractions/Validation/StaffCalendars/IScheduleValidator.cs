using Domain.Entities;
using Domain.ValueObjects.Calendar;

namespace Application.Abstractions.Validation.StaffCalendars;

public interface IScheduleValidator
{
    void EnsureWorkingDayFitsStoreHours(StoreCalendar calendar, WorkingDay workingDay);

    void EnsureExceptionFitsStoreHours(StoreCalendar calendar, CalendarException exception);
}