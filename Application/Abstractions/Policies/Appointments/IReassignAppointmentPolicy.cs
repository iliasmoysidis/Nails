using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IReassignAppointmentPolicy
{
    Task EnsureCanReassignAsync(ReassignAppointmentCommand command, CancellationToken ct);
}