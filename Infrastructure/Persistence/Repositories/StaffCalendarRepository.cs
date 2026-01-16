using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

internal sealed class StaffCalendarRepository : IStaffCalendarRepository
{
    private readonly AppDbContext _db;

    public StaffCalendarRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StaffCalendar> GetByStoreAndProfessionalAsync(int storeId, int professionalId)
    {
        return await _db.StaffCalendars.FirstAsync(c => c.StoreId == storeId && c.ProfessionalId == professionalId);
    }

    public async Task<IReadOnlyCollection<StaffCalendar>> GetAllByProfessionalAsync(int professionalId)
    {
        return await _db.StaffCalendars.Where(c => c.ProfessionalId == professionalId).ToListAsync();
    }

    public async Task SaveAsync(StaffCalendar calendar)
    {
        await _db.SaveChangesAsync();
    }
}