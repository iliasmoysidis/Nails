using Application.Abstractions.Repositories;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public sealed class OfferingRepository : IOfferingRepository
{
    private readonly AppDbContext _db;

    public OfferingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Offering?> GetByIdAsync(int offeringId, CancellationToken ct)
    {
        return await _db.Offerings.FindAsync([offeringId], ct);
    }
}
