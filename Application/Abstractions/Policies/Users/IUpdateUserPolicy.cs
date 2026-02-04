namespace Application.Abstractions.Policies.Users;

public interface IUpdateUserPolicy
{
    Task EnsureCanUpdateAsync(int userId, CancellationToken ct);
}