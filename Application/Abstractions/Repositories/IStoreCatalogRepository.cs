using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetByStoreIdAsync(int storeId, CancellationToken ct);
}