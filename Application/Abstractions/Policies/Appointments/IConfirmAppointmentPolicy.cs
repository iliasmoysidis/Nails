using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IConfirmAppointmentPolicy
{
    Task EnsureCanConfirmAsync(ConfirmAppointmentCommand command, CancellationToken ct);
}