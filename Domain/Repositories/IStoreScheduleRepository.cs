using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreScheduleRepository
{
    Task<StoreScheduleManager> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreScheduleManager manager);
}