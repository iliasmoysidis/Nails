using Domain.Stores;

namespace Application.Stores.Common.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(int storeId, CancellationToken ct);

    Task<IReadOnlyCollection<Store>> GetOwnedStores(int professionalId, CancellationToken ct);

    Task AddAsync(Store store, CancellationToken ct);
}
