using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IReassignPolicy
{
    Task EnsureCanReassignAsync(ReassignCommand command, CancellationToken ct);
}