using Application.Commands.Professionals;

namespace Application.Abstractions.Validation.Professionals;

public interface IRegistrationValidator
{
    Task EnsureUniqueAsync(RegisterCommand command, CancellationToken ct);
}