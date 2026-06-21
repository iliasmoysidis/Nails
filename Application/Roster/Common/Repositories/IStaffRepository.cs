using Domain.Roster;

namespace Application.Roster.Common.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(Staff staff, CancellationToken ct);
}