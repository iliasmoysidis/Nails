namespace Application.Abstractions.Policies.Users;

public interface IManageUserPolicy
{
    Task EnsureCanManageAsync(int userId, CancellationToken ct);
}