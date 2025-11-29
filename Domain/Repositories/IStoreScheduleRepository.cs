using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreScheduleRepository
{
    StoreScheduleManager GetByStoreId(int storeId);
    void Save(StoreScheduleManager manager);
}