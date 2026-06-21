using Application.Calendar.Common.Repositories;
using Infrastructure.Common;
using Domain.Calendar;
using Domain.Common.ValueObjects.Calendar;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar;

public sealed class StoreCalendarRepository : IStoreCalendarRepository
{
    private readonly AppDbContext _db;

    public StoreCalendarRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StoreCalendar?> GetByIdAsync(int storeId, CancellationToken ct)
    {
        var workingRanges = await _db.StoreCalendarWorkingRanges
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        var exceptions = await _db.StoreCalendarExceptions
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        if (!workingRanges.Any() && !exceptions.Any())
            return null;

        return Map(
            storeId: storeId,
            workingRanges: workingRanges,
            exceptions: exceptions
        );
    }

    public async Task RemoveAsync(int storeId, CancellationToken ct)
    {
        await _db.StoreCalendarWorkingRanges
            .Where(x => x.StoreId == storeId)
            .ExecuteDeleteAsync(ct);

        await _db.StoreCalendarExceptions
            .Where(x => x.StoreId == storeId)
            .ExecuteDeleteAsync(ct);
    }

    private static StoreCalendar Map(
        int storeId,
        IEnumerable<StoreCalendarWorkingRangeEntity> workingRanges,
        IEnumerable<StoreCalendarExceptionEntity> exceptions
    )
    {
        var workingDays = workingRanges
            .GroupBy(x => x.Day)
            .Select(g => WorkingDay.WithRanges(
                g.Key,
                g.Select(x => new TimeRange(
                    x.Start,
                    x.End
                    )
                )
            ));

        var groupedExceptions = exceptions
            .GroupBy(x => x.Date)
            .Select(g =>
            {
                var first = g.First();

                if (first.IsDayOff)
                    return CalendarException.DayOff(g.Key);

                return CalendarException.PartialDay(
                    g.Key,
                    g.Select(x => new TimeRange(
                        x.Start!.Value,
                        x.End!.Value
                    ))
                );
            });

        return StoreCalendar.Rehydrate(
            storeId: storeId,
            workingDays: workingDays,
            exceptions: groupedExceptions
        );
    }
}
