using Domain.Schedule.Entities;
using Application.Abstractions.Repositories;
using Domain.Common.ValueObjects.Calendar;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class StaffCalendarRepository : IStaffCalendarRepository
{
    private readonly AppDbContext _db;

    public StaffCalendarRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StaffCalendar?> GetAsync(
        int storeId,
        int professionalId,
        CancellationToken ct)
    {
        var workingRanges = await _db.StaffCalendarWorkingRanges
            .AsNoTracking()
            .Where(x => x.StoreId == storeId &&
                        x.ProfessionalId == professionalId)
            .ToListAsync(ct);

        var exceptions = await _db.StaffCalendarExceptions
            .AsNoTracking()
            .Where(x => x.StoreId == storeId &&
                        x.ProfessionalId == professionalId)
            .ToListAsync(ct);

        if (!workingRanges.Any() && !exceptions.Any())
            return null;

        return Map(
            storeId,
            professionalId,
            workingRanges,
            exceptions);
    }

    public async Task<IReadOnlyCollection<StaffCalendar>> GetByStoreIdAsync(
        int storeId,
        CancellationToken ct)
    {
        var workingRanges = await _db.StaffCalendarWorkingRanges
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        var exceptions = await _db.StaffCalendarExceptions
            .AsNoTracking()
            .Where(x => x.StoreId == storeId)
            .ToListAsync(ct);

        var professionalIds = workingRanges
            .Select(x => x.ProfessionalId)
            .Union(exceptions.Select(x => x.ProfessionalId))
            .Distinct()
            .ToArray();

        var workingLookup = workingRanges
            .ToLookup(x => x.ProfessionalId);

        var exceptionLookup = exceptions
            .ToLookup(x => x.ProfessionalId);

        return professionalIds
            .Select(id => Map(
                storeId,
                id,
                workingLookup[id],
                exceptionLookup[id]))
            .ToArray();
    }

    public async Task RemoveAsync(
        int storeId,
        CancellationToken ct)
    {
        await _db.StaffCalendarWorkingRanges
            .Where(x => x.StoreId == storeId)
            .ExecuteDeleteAsync(ct);

        await _db.StaffCalendarExceptions
            .Where(x => x.StoreId == storeId)
            .ExecuteDeleteAsync(ct);
    }

    public async Task RemoveProfessionalAsync(
        int storeId,
        int professionalId,
        CancellationToken ct)
    {
        await _db.StaffCalendarWorkingRanges
            .Where(x => x.StoreId == storeId &&
                        x.ProfessionalId == professionalId)
            .ExecuteDeleteAsync(ct);

        await _db.StaffCalendarExceptions
            .Where(x => x.StoreId == storeId &&
                        x.ProfessionalId == professionalId)
            .ExecuteDeleteAsync(ct);
    }

    private static StaffCalendar Map(
        int storeId,
        int professionalId,
        IEnumerable<StaffCalendarWorkingRangeEntity> workingRanges,
        IEnumerable<StaffCalendarExceptionEntity> exceptions)
    {
        var workingDays = workingRanges
            .GroupBy(x => x.Day)
            .Select(g => WorkingDay.WithRanges(
                g.Key,
                g.Select(x => new TimeRange(
                    x.Start,
                    x.End))
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
                        x.End!.Value))
                );
            });

        return StaffCalendar.Rehydrate(
            storeId,
            professionalId,
            workingDays,
            groupedExceptions);
    }
}