using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(Store store, CancellationToken ct);
}