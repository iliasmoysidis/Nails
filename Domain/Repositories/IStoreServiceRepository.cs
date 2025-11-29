using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreServiceRepository
{
    Task<StoreServiceManager> GetByStoreIdAsync(int storeId);
    Task SaveAsync(StoreServiceManager manager);
}