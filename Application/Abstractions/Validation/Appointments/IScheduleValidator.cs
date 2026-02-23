using Application.Commands.Appointments;

namespace Application.Abstractions.Validation.Appointments;

public interface IScheduleValidator
{
    Task EnsureAvailableAsync(ScheduleCommand command, CancellationToken ct);
}