using Application.Schedules.Common.Repositories;
using Domain.Schedules;
using Domain.Schedules.Entities;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Schedules;

public sealed class ProfessionalScheduleRepository : IProfessionalScheduleRepository
{
    private readonly AppDbContext _context;

    public ProfessionalScheduleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProfessionalSchedule?> GetByProfessionalIdAsync(int professionalId, CancellationToken ct)
    {
        return await _context.ProfessionalSchedules.FirstOrDefaultAsync(s => s.ProfessionalId == professionalId, ct);
    }

    public async Task<IReadOnlyCollection<ProfessionalSchedule>> GetByStoreIdAsync(int storeId, CancellationToken ct)
    {
        return await _context.ProfessionalSchedules.Where(s => s.Calendars.Any(c => c.StoreId == storeId)).ToListAsync(ct);
    }
}
