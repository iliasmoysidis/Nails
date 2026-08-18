using Application.Rosters.Common.Repositories;
using Domain.Rosters;
using Domain.Rosters.EnumObjects;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Rosters;

public sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _context;

    public StaffRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Staff staff, CancellationToken ct)
    {
        await _context.Staff.AddAsync(staff, ct);
    }

    public async Task<Staff?> GetByStoreIdAsync(int storeId, CancellationToken ct)
    {
        return await _context.Staff.FirstOrDefaultAsync(s => s.StoreId == storeId, ct);
    }

    public async Task<bool> IsOwnerAsync(int storeId, int professionalid, CancellationToken ct)
    {
        return await _context.Staff
            .Where(s => s.StoreId == storeId)
            .SelectMany(s => s.Members)
            .AnyAsync(m => m.ProfessionalId == professionalid && m.HasRole(StaffRole.Owner));
    }

    public async Task<bool> IsStaffMemberAsync(int storeId, int professionalid, CancellationToken ct)
    {
        return await _context.Staff
            .Where(s => s.StoreId == storeId)
            .SelectMany(s => s.Members)
            .AnyAsync(m => m.ProfessionalId == professionalid, ct);
    }
}
