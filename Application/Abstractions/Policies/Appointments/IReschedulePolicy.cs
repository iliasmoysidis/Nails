using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IReschedulePolicy
{
    Task EnsureCanRescheduleAsync(RescheduleCommand command, CancellationToken ct);
}