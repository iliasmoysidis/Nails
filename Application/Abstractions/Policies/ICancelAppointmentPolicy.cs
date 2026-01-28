using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface ICancelAppointmentPolicy
{
    Task EnsureCanCancelAsync(CancelAppointmentCommand command, CancellationToken ct);
}