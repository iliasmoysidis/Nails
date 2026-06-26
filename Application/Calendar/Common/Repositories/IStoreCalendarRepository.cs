using Domain.Calendar;

namespace Application.Calendar.Common.Repositories;

public interface IStoreCalendarRepository
{
    Task<StoreCalendar?> GetByIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(StoreCalendar calendar, CancellationToken ct);
}
