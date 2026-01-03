using Domain.Entities;
using Domain.Repositories;

namespace Domain.Tests.Fakes;

public sealed class FakeStoreCalendarRepository : IStoreCalendarRepository
{
    private readonly StoreCalendar _calendar;

    public FakeStoreCalendarRepository(StoreCalendar calendar)
    {
        _calendar = calendar;
    }

    public Task<StoreCalendar> GetByStoreAsync(int storeId)
        => Task.FromResult(_calendar);

    public Task SaveAsync(StoreCalendar calendar)
        => Task.CompletedTask;
}