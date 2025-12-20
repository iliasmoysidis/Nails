using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects.Time;

namespace Domain.Policies;

public sealed class AppointmentOverlapPolicy
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentOverlapPolicy(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task EnsureNoConflictAsync(int professionalId, UtcDateTime start, UtcDateTime end, int? excludeAppointmentId = null)
    {
        var appointments = await _appointmentRepository.GetByProfessionalAsync(professionalId);

        bool hasConflict = appointments.Any(a =>
            a.Id != excludeAppointmentId &&
            !a.IsDeleted &&
            a.ConflictsWith(start, end)
            );

        if (hasConflict)
            throw new DomainException("Professional already has an appointment at this time.");
    }
}