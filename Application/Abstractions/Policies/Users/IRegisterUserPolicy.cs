using Domain.ValueObjects.Identity;

namespace Application.Abstractions.Policies.Users;

public interface IRegisterUserPolicy
{
    Task EnsureCanRegisterAsync(Email email, Phone phone, CancellationToken ct);
}