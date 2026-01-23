namespace Application.Policies.Interfaces;

public interface IStoreOwnerAccessPolicy
{
    Task EnsureIsOwnerAsync(int userId, int storeId, CancellationToken ct);
}