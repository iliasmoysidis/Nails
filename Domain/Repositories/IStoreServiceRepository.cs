using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreServiceRepository
{
    Task<StoreServiceManager> GetByStoreId(int storeId);
    Task Save(StoreServiceManager manager);
}