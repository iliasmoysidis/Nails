namespace Application.Abstractions.Policies.Appointments;

public interface IUpdateAppointmentNotesPolicy
{
    Task EnsureCanUpdateNotesAsync(int appointmentId, CancellationToken ct);
}