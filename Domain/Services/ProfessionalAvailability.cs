using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public class ProfessionalAvailability
{
    public int StoreId { get; }
    public int ProfessionalId { get; }

    public StoreCalendar StoreCalendar { get; }
    public StaffCalendar StaffCalendar { get; }

    private readonly ProfessionalSchedule _professionalSchedule;

    public ProfessionalAvailability(
        StoreCalendar storeCalendar,
        ProfessionalSchedule professionalSchedule,
        int storeId
    )
    {
        var staffCalendar = professionalSchedule.GetCalendar(storeId);

        if (staffCalendar.StoreId != storeCalendar.StoreId)
            throw new InvariantException("Calendar mismatch.");

        StoreId = storeCalendar.StoreId;
        ProfessionalId = staffCalendar.ProfessionalId;
        StoreCalendar = storeCalendar;
        StaffCalendar = staffCalendar;
        _professionalSchedule = professionalSchedule;
    }

    public void SetWorkingDay(WorkingDay workingDay)
    {
        EnsureFitsStoreHours(workingDay);

        EnsureNoProfessionalConflicts(workingDay);

        StaffCalendar.SetWorkingDay(workingDay);
    }

    public void SetException(CalendarException exception)
    {
        EnsureFitsStoreHours(exception);

        EnsureNoProfessionalConflicts(exception);

        StaffCalendar.SetException(exception);
    }

    public void SetDayOff(DayOfWeek day)
    {
        StaffCalendar.SetDayOff(day);
    }

    public void RemoveException(DateOnly date)
    {
        StaffCalendar.RemoveException(date);
    }

    private void EnsureFitsStoreHours(
    WorkingDay workingDay)
    {
        if (workingDay.IsDayOff)
            return;

        foreach (var range in workingDay.TimeRanges)
        {
            if (!StoreCalendar.IsWithinWeeklyStoreHours(
                workingDay.Day,
                range))
            {
                throw new InvariantException(
                    "Working schedule must fit within store hours.");
            }

        }
    }

    private void EnsureFitsStoreHours(CalendarException exception)
    {
        if (exception.IsDayOff)
            return;

        foreach (var range in exception.TimeRanges)
        {
            if (!StoreCalendar.IsWithinStoreHours(exception.Date, range))
                throw new InvariantException("Special availability must fit within store hours.");
        }
    }

    private void EnsureNoProfessionalConflicts(WorkingDay workingDay)
    {
        _professionalSchedule.EnsureWorkingDayDoesNotConflict(StoreId, workingDay);
    }

    private void EnsureNoProfessionalConflicts(CalendarException exception)
    {
        _professionalSchedule.EnsureExceptionDoesNotConflict(StoreId, exception);
    }
}
