using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Services;

public sealed class EmploymentTermination
{
    public int StoreId { get; }
    public int ProfessionalId { get; }

    private readonly Staff _staff;
    private readonly Assignments _assignments;
    private readonly ProfessionalSchedule _professionalSchedule;
    private readonly IReadOnlyCollection<Appointment> _appointments;

    public EmploymentTermination(
        int storeId,
        int professionalId,
        Staff staff,
        Assignments assignments,
        ProfessionalSchedule professionalSchedule,
        IReadOnlyCollection<Appointment> appointments
    )
    {
        if (staff.StoreId != storeId)
            throw new InvariantException("Staff does not belong to this store");

        if (assignments.StoreId != storeId)
            throw new InvariantException("Assignments do not belong to this store.");

        if (professionalSchedule.ProfessionalId != professionalId)
            throw new InvariantException("Schedule does not belong to this professional.");

        foreach (var appointment in appointments)
        {
            if (appointment.StoreId != storeId)
                throw new InvariantException("Appointment does not belong to this store.");

            if (appointment.ProfessionalId != professionalId)
                throw new InvariantException("Appointment does not belong to this professional.");
        }

        StoreId = storeId;
        ProfessionalId = professionalId;

        _staff = staff;
        _assignments = assignments;
        _professionalSchedule = professionalSchedule;
        _appointments = appointments;
    }

    public void Terminate(IClock clock)
    {
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
}
