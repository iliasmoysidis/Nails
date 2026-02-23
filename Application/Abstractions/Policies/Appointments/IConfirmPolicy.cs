using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IConfirmPolicy
{
    Task EnsureCanConfirmAsync(ConfirmCommand command, CancellationToken ct);
}