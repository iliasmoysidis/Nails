using Domain.Rosters;

namespace Application.Rosters.Common.Repositories;

public interface IStaffRepository
{
    Task<Staff?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task<bool> IsStaffMemberAsync(
        int storeId,
        int professionalid,
        CancellationToken ct
    );

    Task<bool> IsOwnerAsync(int storeId, int professionalid, CancellationToken ct);

    Task AddAsync(Staff staff, CancellationToken ct);
}
