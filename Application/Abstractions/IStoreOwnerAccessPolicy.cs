namespace Application.Abstractions;

public interface IStoreOwnerAccessPolicy
{
    Task EnsureIsOwnerAsync(int userId, int storeId, CancellationToken ct);
}