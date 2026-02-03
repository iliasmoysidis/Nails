namespace Application.Abstractions.Policies.Stores;

public interface IManageStorePolicy
{
    Task EnsureCanManageAsync(int storeId, CancellationToken ct);
}