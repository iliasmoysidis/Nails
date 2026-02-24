using Domain.Entities;
using Domain.ValueObjects.Calendar;

namespace Domain.Interfaces;

public interface ISchedulingService
{
    void EnsureWorkingDayFitsStoreHours(
        StoreCalendar calendar,
        WorkingDay workingDay
    );

    void EnsureExceptionFitsStoreHours(
        StoreCalendar calendar,
        CalendarException exception
    );
}