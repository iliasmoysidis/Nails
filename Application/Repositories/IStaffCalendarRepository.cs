namespace Application.Repositories;

using Domain.Entities;

public interface IStaffCalendarRepository
{
    Task<StaffCalendar?> GetAsync(int storeId, int professionalId, CancellationToken ct);

    Task<IReadOnlyCollection<StaffCalendar>> GetOtherCalendarsAsync(int storeId, int professionalId, CancellationToken ct);
    Task<Staff> GetStaffAsync(int storeId, CancellationToken ct);
}