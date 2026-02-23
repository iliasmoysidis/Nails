using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface ICancelPolicy
{
    Task EnsureCanCancelAsync(CancelCommand command, CancellationToken ct);
}