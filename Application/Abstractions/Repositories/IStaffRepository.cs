using Domain.Roster;

namespace Application.Abstractions.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(Staff staff, CancellationToken ct);
}