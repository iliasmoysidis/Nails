using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IStaffCalendarRepository
{
    Task<StaffCalendar?> GetAsync(int storeId, int professionalId, CancellationToken ct);
}