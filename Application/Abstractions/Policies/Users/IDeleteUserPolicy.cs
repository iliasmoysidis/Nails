namespace Application.Abstractions.Policies.Users;

public interface IDeleteUserPolicy
{
    Task EnsureCanDeleteAsync(int userId, CancellationToken ct);
}