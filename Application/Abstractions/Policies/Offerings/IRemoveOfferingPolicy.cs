using Application.Commands.Offerings;

namespace Application.Abstractions.Policies.Offerings;

public interface IRemoveOfferingPolicy
{
    Task EnsureCanRemoveAsync(RemoveOfferingCommand command, CancellationToken ct);
}