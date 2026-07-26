using Application.Common.DTO;
using Application.Schedule.Common.Queries;
using Application.Schedule.GetExceptions;
using Application.Schedule.GetWeeklySchedule;
using Infrastructure.Common;
using Infrastructure.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Schedule;

public sealed class ProfessionalScheduleQueries : IProfessionalScheduleQueries
{
    private readonly AppDbContext _context;

    public ProfessionalScheduleQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<StaffCalendarExceptionDTO>> GetExceptionsAsync(
        int storeId,
        int professionalId,
        DateOnly from,
        DateOnly to,
        int? page,
        int? limit,
        CancellationToken ct
    )
    {
        return await _context.ProfessionalSchedules
            .Where(ps => ps.ProfessionalId == professionalId)
            .SelectMany(ps => ps.Calendars)
            .Where(sc => sc.StoreId == storeId)
            .SelectMany(sc => sc.Exceptions)
            .Where(e => e.Date >= from && e.Date <= to)
            .OrderBy(e => e.Date)
            .Select(
                e => new StaffCalendarExceptionDTO(
                    e.Date,
                    e.IsDayOff,
                    e.TimeRanges
                        .OrderBy(r => r.Start)
                        .Select(
                            r => new TimeRangeDTO(
                                r.Start,
                                r.End
                            )
                        )
                        .ToList()
                )
            )
            .ToPagedResultAsync(page, limit, ct);
    }

    public async Task<IReadOnlyCollection<StaffWorkingDayDTO>> GetWeeklyScheduleAsync(int storeId, int professionalId, CancellationToken ct)
    {
        return await _context.ProfessionalSchedules
            .Where(ps => ps.ProfessionalId == professionalId)
            .SelectMany(ps => ps.Calendars)
            .Where(sc => sc.StoreId == storeId)
            .SelectMany(sc => sc.WorkingDays)
            .OrderBy(wd => wd.Day)
            .Select(
                wd => new StaffWorkingDayDTO(
                    wd.Day,
                    wd.IsDayOff,
                    wd.TimeRanges
                        .OrderBy(r => r.Start)
                        .Select(
                            r => new TimeRangeDTO(
                                r.Start,
                                r.End
                            )
                        )
                        .ToList()
                )
            )
            .ToListAsync(ct);
    }
}
