using Application.Stores.Common.Repositories;
using Infrastructure.Common;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Stores;

public sealed class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _db;

    public StoreRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Store store, CancellationToken ct)
    {
        await _db.Stores.AddAsync(store, ct);
    }

    public async Task<Store?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        return await _db.Stores.AsNoTracking().FirstOrDefaultAsync(x => x.Id == storeId, ct);
    }
}
