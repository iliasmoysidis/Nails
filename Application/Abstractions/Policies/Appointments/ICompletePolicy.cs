using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface ICompletePolicy
{
    Task EnsureCanCompleteAsync(CompleteCommand command, CancellationToken ct);
}