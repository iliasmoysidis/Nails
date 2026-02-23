using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public sealed class SchedulingService
{
    public static void EnsureFitsStoreHours(
        StoreCalendar storeCalendar,
        WorkingDay workingDay
    )
    {
        if (workingDay.IsDayOff)
            return;

        var day = workingDay.Day;

        foreach (var range in workingDay.TimeRanges)
        {
            if (!storeCalendar.IsWhithinWeeklyStoreHours(day, range))
                throw new InvariantException("Staff working hours must be within store opening hours.");
        }
    }
}