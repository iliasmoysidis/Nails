using Domain.Entities;
using Domain.Repositories;

namespace Domain.Tests.Fakes;

public sealed class FakeStaffCalendarRepository : IStaffCalendarRepository
{
    private readonly StaffCalendar _calendar;

    public FakeStaffCalendarRepository(StaffCalendar calendar)
    {
        _calendar = calendar;
    }

    public Task<StaffCalendar> GetByStoreAndProfessionalAsync(int storeId, int professionalId)
        => Task.FromResult(_calendar);

    public Task<IReadOnlyCollection<StaffCalendar>> GetAllByProfessionalAsync(int professionalId)
        => Task.FromResult<IReadOnlyCollection<StaffCalendar>>([_calendar]);

    public Task SaveAsync(StaffCalendar calendar)
        => Task.CompletedTask;
}