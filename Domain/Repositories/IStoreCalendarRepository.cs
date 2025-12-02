using Domain.Entities;

namespace Domain.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar> GetByStoreAsync(int storeId);
    Task SaveAsync(StoreCalendar manager);
}