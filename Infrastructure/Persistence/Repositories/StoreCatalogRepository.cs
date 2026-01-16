using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class StoreCatalogRepository : IStoreCatalogRepository
{
    private readonly AppDbContext _db;

    public StoreCatalogRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StoreCatalog> GetByStoreAsync(int storeId)
    {
        return await _db.StoreCatalogs.FirstAsync(c => c.StoreId == storeId);
    }

    public async Task SaveAsync(StoreCatalog catalog)
    {
        await _db.SaveChangesAsync();
    }
}