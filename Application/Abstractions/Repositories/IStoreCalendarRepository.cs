using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar?> GetByIdAsync(int storeId, CancellationToken ct);

    Task RemoveAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCalendar calendar, CancellationToken ct);
}
