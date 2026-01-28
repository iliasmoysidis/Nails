using Application.Commands.Appointments;

namespace Application.Abstractions.Policies;

public interface IConfirmAppointmentPolicy
{
    Task EnsureCanConfirmAsync(ConfirmAppointmentCommand command, CancellationToken ct);
}