namespace Application.Abstractions.Policies.Professionals;

public interface ILeaveStorePolicy
{
    Task EnsureCanLeaveAsync(int storeId, CancellationToken ct);
}