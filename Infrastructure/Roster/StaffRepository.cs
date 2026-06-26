using Application.Roster.Common.Repositories;
using Domain.Roster;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Roster;

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
}
