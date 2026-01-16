using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreCatalogRepository
{
    Task<StoreCatalog?> GetByStoreAsync(int storeId);
    void Add(StoreCatalog catalog);
}