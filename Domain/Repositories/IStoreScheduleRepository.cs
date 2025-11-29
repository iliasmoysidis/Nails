using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreScheduleRepository
{
    Task<StoreScheduleManager> GetByStoreIdAsync(int storeId);
    Task SaveAsync(StoreScheduleManager manager);
}