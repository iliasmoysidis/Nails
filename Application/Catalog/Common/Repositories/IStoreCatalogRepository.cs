using Domain.Catalog;

namespace Application.Catalog.Common.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCatalog catalog, CancellationToken ct);
}
