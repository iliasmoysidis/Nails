using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IAssignmentsRepository
{
    Task<Assignments?> GetByStoreIdAsync(int storeId, CancellationToken ct);

    Task AddAsync(Assignments assignments, CancellationToken ct);
}
