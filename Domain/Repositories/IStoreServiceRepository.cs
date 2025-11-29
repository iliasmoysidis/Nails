using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreServiceRepository
{
    public StoreServiceManager GetByStoreId(int storeId);
    void Save(StoreServiceManager manager);
}