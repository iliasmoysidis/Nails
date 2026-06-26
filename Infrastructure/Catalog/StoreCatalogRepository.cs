using Application.Catalog.Common.Repositories;
using Infrastructure.Common;
using Domain.Catalog;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Catalog;

public sealed class StoreCatalogRepository : IStoreCatalogRepository
{
    private readonly AppDbContext _context;

    public StoreCatalogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StoreCatalog catalog, CancellationToken ct)
    {
        await _context.StoreCatalogs.AddAsync(catalog, ct);
    }

    public async Task<StoreCatalog?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        return await _context.StoreCatalogs.FirstOrDefaultAsync(c => c.StoreId == storeId, ct);
    }
}
