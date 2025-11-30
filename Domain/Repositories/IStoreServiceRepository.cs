using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreServiceRepository
{
    Task<StoreServiceManager> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreServiceManager manager);
}