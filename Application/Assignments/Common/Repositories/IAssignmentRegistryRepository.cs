using Domain.Assignments;

namespace Application.Assignments.Common.Repositories;

public interface IAssignmentRegistryRepository
{
    Task<AssignmentRegistry?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(AssignmentRegistry assignments, CancellationToken ct);
}
