using Domain.Schedule;
using Domain.Assignments;
using Domain.Appointments;
using Domain.Stores;
using Domain.Common;
using Domain.Common.Exceptions;

namespace Domain.Roster.Services;

public sealed class EmploymentTermination
{
    private readonly Store _store;
    private readonly Staff _staff;
    private readonly AssignmentRegistry _assignments;
    private readonly ProfessionalSchedule _professionalSchedule;
    private readonly IReadOnlyCollection<Appointment> _appointments;

    private int ProfessionalId => _professionalSchedule.ProfessionalId;
    private int StoreId => _store.Id;

    public EmploymentTermination(
        Store store,
        Staff staff,
        AssignmentRegistry assignments,
        ProfessionalSchedule professionalSchedule,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        ValidateComposition(
            store,
            staff,
            assignments,
            professionalSchedule,
            appointments
        );

        _store = store;
        _staff = staff;
        _assignments = assignments;
        _professionalSchedule = professionalSchedule;
        _appointments = appointments;
    }

    public void Terminate(IClock clock)
    {
        _store.EnsureOpen();
        _staff.RemoveFromStaff(ProfessionalId);
        _assignments.RemoveByProfessional(ProfessionalId);
        _professionalSchedule.RemoveCalendar(StoreId);

        foreach (var appointment in _appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(clock, "Professional left the store.");
        }
    }

    private void ValidateComposition(
        Store store,
        Staff staff,
        AssignmentRegistry assignments,
        ProfessionalSchedule professionalSchedule,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        if (staff.StoreId != store.Id)
            throw new InvariantException("Staff does not belong to this store");

        if (assignments.StoreId != store.Id)
            throw new InvariantException("Assignments do not belong to this store.");

        if (!staff.IsStaff(professionalSchedule.ProfessionalId))
            throw new InvariantException("Professional does not belong to this store.");

        foreach (var appointment in appointments)
        {
            if (appointment.StoreId != store.Id)
                throw new InvariantException("Appointment does not belong to this store.");

            if (appointment.ProfessionalId != professionalSchedule.ProfessionalId)
                throw new InvariantException("Appointment does not belong to this professional.");
        }
    }
}
