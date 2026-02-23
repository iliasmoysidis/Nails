using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IReassignValidator
{
    Task EnsureAvailableAsync(ReassignCommand command, CancellationToken ct);
}