using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class StaffRepository : IStaffRepository
{
    private readonly AppDbContext _db;

    public StaffRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Staff> GetByStoreAsync(int storeId)
    {
        return await _db.Staffs.FirstAsync(s => s.StoreId == storeId);
    }

    public async Task SaveAsync(Staff staff)
    {
        await _db.SaveChangesAsync();
    }
}