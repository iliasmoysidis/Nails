using Domain.Catalogs;

namespace Application.Catalogs.Common.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCatalog catalog, CancellationToken ct);
}
