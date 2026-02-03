using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface ICompleteAppointmentPolicy
{
    Task EnsureCanCompleteAsync(CompleteAppointmentCommand command, CancellationToken ct);
}