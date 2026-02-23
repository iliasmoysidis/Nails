using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IRescheduleValidator
{
    Task EnsureAvailableAsync(RescheduleCommand command, CancellationToken ct);
}