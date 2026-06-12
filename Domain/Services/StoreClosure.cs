using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class StoreClosure
{
    private readonly Store _store;
    private readonly Staff _staff;
    private readonly Assignments _assignments;
    private readonly IReadOnlyCollection<ProfessionalSchedule> _professionalSchedules;
    private readonly IReadOnlyCollection<Appointment> _appointments;

    public StoreClosure(
        Store store,
        Staff staff,
        Assignments assignments,
        IReadOnlyCollection<ProfessionalSchedule> professionalSchedules,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        ValidateComposition(
            store,
            staff,
            assignments,
            professionalSchedules,
            appointments
        );

        _store = store;
        _staff = staff;
        _assignments = assignments;
        _professionalSchedules = professionalSchedules;
        _appointments = appointments;
    }

    public void Close(IClock clock)
    {
        _store.EnsureOpen();

        foreach (var appointment in _appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(clock, "Store permanently closed.");
        }

        foreach (var schedule in _professionalSchedules)
        {
            schedule.RemoveCalendar(_store.Id);
        }

        _assignments.Clear();

        _staff.Clear();

        _store.Close(clock);
    }

    private void ValidateComposition(
        Store store,
        Staff staff,
        Assignments assignments,
        IReadOnlyCollection<ProfessionalSchedule> professionalSchedules,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        if (staff.StoreId != store.Id)
            throw new InvariantException("Staff does not belong to this store.");

        if (assignments.StoreId != store.Id)
            throw new InvariantException("Assignments do not belong to this store.");

        foreach (var professionalSchedule in professionalSchedules)
        {
            if (!staff.IsStaff(professionalSchedule.ProfessionalId))
                throw new InvariantException("Professional does not belong to this store.");
        }

        foreach (var appointment in appointments)
        {
            if (appointment.StoreId != store.Id)
                throw new InvariantException("Appointment does not belong to this store.");
        }
    }
}
