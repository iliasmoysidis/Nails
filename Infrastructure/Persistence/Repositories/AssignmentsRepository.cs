using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class AssignmentsRepository : IAssignmentsRepository
{
    private readonly AppDbContext _db;

    public AssignmentsRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Assignments?> GetByStoreIdAsync(int storeId, CancellationToken ct)
    {
        var rows = await _db.Assignments.Where(a => a.StoreId == storeId).ToListAsync(ct);

        var assignmets = Assignments.Create(storeId);

        foreach (var row in rows)
        {
            assignmets.Add(row.ProfessionalId, row.OfferingId);
        }

        return assignmets;
    }
}