using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface ICompleteAppointmentPolicy
{
    Task EnsureCanCompleteAsync(CompleteAppointmentCommand command, CancellationToken ct);
}