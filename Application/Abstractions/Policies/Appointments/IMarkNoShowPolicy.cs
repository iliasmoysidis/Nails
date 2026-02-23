using Application.Commands.Appointments;

namespace Application.Abstractions.Policies.Appointments;

public interface IMarkNoShowPolicy
{
    Task EnsureCanMarkNoShowAsync(MarkNoShowCommand command, CancellationToken ct);
}