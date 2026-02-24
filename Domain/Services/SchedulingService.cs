using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public sealed class SchedulingService : ISchedulingService
{
    public void EnsureWorkingDayFitsStoreHours(
        StoreCalendar calendar,
        WorkingDay workingDay
    )
    {
        if (workingDay.IsDayOff)
            return;

        foreach (var range in workingDay.TimeRanges)
        {
            if (!calendar.IsWithinWeeklyStoreHours(workingDay.Day, range))
                throw new InvariantException("Staff working schedule must fit within store opening hours.");
        }
    }

    public void EnsureExceptionFitsStoreHours(
        StoreCalendar calendar,
        CalendarException exception
    )
    {
        if (exception.IsDayOff)
            return;

        foreach (var range in exception.TimeRanges)
        {
            if (!calendar.IsWithinStoreHours(exception.Date, range))
                throw new InvariantException(
                    "Special availability must be within store operating hours.");
        }
    }
}