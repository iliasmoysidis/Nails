using Application.Catalog.Common.Repositories;
using Infrastructure.Common;
using Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Catalog;

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
