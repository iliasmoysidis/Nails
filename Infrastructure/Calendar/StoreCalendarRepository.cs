using Application.Calendar.Common.Repositories;
using Domain.Calendar;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar;

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

    public async Task RemoveAsync(int storeId, CancellationToken ct)
    {
        var calendar = await GetByIdAsync(storeId, ct);

        if (calendar is null)
            return;

        _context.StoreCalendars.Remove(calendar);
    }
}
