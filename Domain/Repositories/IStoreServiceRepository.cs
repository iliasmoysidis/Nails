using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreServiceRepository
{
    Task<StoreCatalog> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreCatalog manager);
}