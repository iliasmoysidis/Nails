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
        var rows = await _db.Assignments.Where(a => a.StoreId == storeId).ToListAsync(ct);

        var assignmets = AssignmentRegistry.Create(storeId);

        foreach (var row in rows)
        {
            assignmets.Add(row.ProfessionalId, row.OfferingId);
        }

        return assignmets;
    }
}