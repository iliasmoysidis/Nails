using Application.Assignments.Common.Repositories;
using Infrastructure.Common;
using Domain.Assignments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Assignments;

public sealed class AssignmentsRepository : IAssignmentsRepository
{
    private readonly AppDbContext _db;

    public AssignmentsRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AssignmentRegistry?> GetByStoreIdAsync(int storeId, CancellationToken ct)
    {
        return await _db.Assignments.FirstOrDefaultAsync(x => x.StoreId == storeId, ct);
    }

    public async Task AddAsync(AssignmentRegistry assignments, CancellationToken ct)
    {
        await _db.Assignments.AddAsync(assignments, ct);
    }
}
