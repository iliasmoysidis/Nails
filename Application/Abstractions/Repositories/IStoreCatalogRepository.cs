using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCatalog catalog, CancellationToken ct);
}
