using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreScheduleRepository
{
    Task<StoreScheduleManager> GetByStoreId(int storeId);
    Task Save(StoreScheduleManager manager);
}