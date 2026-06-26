using Application.Stores.Common.Repositories;
using Infrastructure.Common;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Domain.Roster.EnumObjects;

namespace Infrastructure.Stores;

public sealed class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Store store, CancellationToken ct)
    {
        await _context.Stores.AddAsync(store, ct);
    }

    public async Task<Store?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        return await _context.Stores.FirstOrDefaultAsync(x => x.Id == storeId, ct);
    }

    public async Task<IReadOnlyCollection<Store>> GetOwnedStores(int professionalId, CancellationToken ct)
    {
        var ownedStoreIds = await _context.Staff.Where(s => s.Members.Any(m => m.ProfessionalId == professionalId && m.Roles.Any(r => r.Role == StaffRole.Owner))).Select(s => s.StoreId).ToListAsync(ct);

        return await _context.Stores.Where(s => ownedStoreIds.Contains(s.Id)).ToListAsync(ct);
    }
}
