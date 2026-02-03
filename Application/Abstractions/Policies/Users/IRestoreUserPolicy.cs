namespace Application.Abstractions.Policies.Users;

public interface IRestoreUserPolicy
{
    Task EnsureCanRestoreAsync(int userId, CancellationToken ct);
}