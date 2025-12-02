using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreScheduleRepository
{
    Task<StoreCalendar> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreCalendar manager);
}