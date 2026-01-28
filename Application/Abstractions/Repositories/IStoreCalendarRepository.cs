using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar?> GetByStoreIdAsync(int storeId, CancellationToken ct);
}