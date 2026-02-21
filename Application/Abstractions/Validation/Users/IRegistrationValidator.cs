using Application.Commands.Users;

namespace Application.Abstractions.Validation.Users;

public interface IRegistrationValidator
{
    Task EnsureUniqueAsync(RegisterCommand command, CancellationToken ct);
}