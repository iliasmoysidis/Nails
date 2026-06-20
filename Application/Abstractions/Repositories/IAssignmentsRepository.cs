using Domain.Assignments;

namespace Application.Abstractions.Repositories;

public interface IAssignmentsRepository
{
    Task<AssignmentRegistry?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(AssignmentRegistry assignments, CancellationToken ct);
}
