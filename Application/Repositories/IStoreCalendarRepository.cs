using Domain.Entities;

namespace Application.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar?> GetAsync(int storeId, CancellationToken ct);
    Task<Staff> GetStaffAsync(int storeId, CancellationToken ct);
}