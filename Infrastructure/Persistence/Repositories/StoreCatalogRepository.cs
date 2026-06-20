using Domain.Catalog;
using Application.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class StoreCatalogRepository : IStoreCatalogRepository
{
    private readonly AppDbContext _db;

    public StoreCatalogRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StoreCatalog?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        var offerings = await _db.Offerings
            .Where(o => o.StoreId == storeId)
            .ToListAsync();

        if (!offerings.Any())
            return null;

        return StoreCatalog.Rehydrate(
            storeId: storeId,
            offerings: offerings
        );
    }
}
