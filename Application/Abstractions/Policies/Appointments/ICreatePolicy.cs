using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface ICreatePolicy
{
    Task EnsureCanCreateAsync(ScheduleCommand command, CancellationToken ct);
}