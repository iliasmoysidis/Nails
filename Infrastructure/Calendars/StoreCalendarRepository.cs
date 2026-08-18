using Application.Calendars.Common.Repositories;
using Domain.Calendars;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendars;

public sealed class StoreCalendarRepository : IStoreCalendarRepository
{
    private readonly AppDbContext _context;

    public StoreCalendarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(StoreCalendar calendar, CancellationToken ct)
    {
        await _context.StoreCalendars.AddAsync(calendar, ct);
    }

    public async Task<StoreCalendar?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        return await _context.StoreCalendars.FirstOrDefaultAsync(c => c.StoreId == storeId, ct);
    }
}
