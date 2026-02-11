using Application.Commands.Offerings;

namespace Application.Abstractions.Policies.Offerings;

public interface ICreateOfferingPolicy
{
    Task EnsureCanCreateAsync(CreateOfferingCommand command, CancellationToken ct);
}