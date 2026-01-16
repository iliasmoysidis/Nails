using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class StoreCalendarRepository : IStoreCalendarRepository
{
    private readonly AppDbContext _db;

    public StoreCalendarRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StoreCalendar> GetByStoreAsync(int storeId)
    {
        return await _db.StoreCalendars.FirstAsync(c => c.StoreId == storeId);
    }

    public async Task SaveAsync(StoreCalendar calendar)
    {
        await _db.SaveChangesAsync();
    }
}