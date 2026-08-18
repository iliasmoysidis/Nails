using Domain.Calendars;

namespace Application.Calendars.Common.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCalendar calendar, CancellationToken ct);
}
