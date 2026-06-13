using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public sealed class ProfessionalAvailability
{
    private readonly Professional _professional;
    private readonly Store _store;
    private readonly StoreCalendar _storeCalendar;
    private readonly StaffCalendar _staffCalendar;
    private readonly ProfessionalSchedule _professionalSchedule;

    public ProfessionalAvailability(
        Professional professional,
        Store store,
        StoreCalendar storeCalendar,
        ProfessionalSchedule professionalSchedule
    )
    {
        var staffCalendar = professionalSchedule.GetCalendar(store.Id);

        ValidateComposition(professional, store, storeCalendar, staffCalendar);

        _professional = professional;
        _store = store;
        _storeCalendar = storeCalendar;
        _staffCalendar = staffCalendar;
        _professionalSchedule = professionalSchedule;
    }

    public void SetWorkingDay(WorkingDay workingDay)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();
        EnsureFitsStoreHours(workingDay);
        EnsureNoProfessionalConflicts(workingDay);

        _staffCalendar.SetWorkingHours(workingDay);
    }

    public void SetException(CalendarException exception)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();
        EnsureFitsStoreHours(exception);
        EnsureNoProfessionalConflicts(exception);

        _staffCalendar.SetSpecialAvailability(exception);
    }

    public void SetDayOff(DayOfWeek day)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();

        _staffCalendar.RestDay(day);
    }

    public void RemoveException(DateOnly date)
    {
        _professional.EnsureActive();
        _store.EnsureOpen();

        _staffCalendar.RemoveSpecialAvailability(date);
    }

    private void EnsureFitsStoreHours(
    WorkingDay workingDay)
    {
        if (workingDay.IsDayOff)
            return;

        foreach (var range in workingDay.TimeRanges)
        {
            if (!_storeCalendar.IsWithinWeeklyStoreHours(workingDay.Day, range))
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
            if (!_storeCalendar.IsWithinStoreHours(exception.Date, range))
                throw new InvariantException("Special availability must fit within store hours.");
        }
    }

    private void EnsureNoProfessionalConflicts(WorkingDay workingDay)
    {
        _professionalSchedule.EnsureWorkingDayDoesNotConflict(_store.Id, workingDay);
    }

    private void EnsureNoProfessionalConflicts(CalendarException exception)
    {
        _professionalSchedule.EnsureExceptionDoesNotConflict(_store.Id, exception);
    }

    private void ValidateComposition(
        Professional professional,
        Store store,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar
    )
    {
        if (storeCalendar.StoreId != store.Id)
            throw new InvariantException(
                "Store calendar does not belong to this store.");

        if (staffCalendar.StoreId != store.Id)
            throw new InvariantException("Staff calendar does not belong to this store.");

        if (staffCalendar.ProfessionalId != professional.Id)
            throw new InvariantException("Staff calendar does not belong to professional.");
    }
}
