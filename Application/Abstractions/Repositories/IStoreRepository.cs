using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(Store store, CancellationToken ct);
}