using Application.Calendar.Common.DTO;
using Application.Calendar.Common.Queries;
using Application.Calendar.GetAvailability;
using Application.Common.DTO;
using Domain.Common.ValueObjects;
using Domain.Common.ValueObjects.Calendar;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Calendar;

public sealed class StoreCalendarQueries : IStoreCalendarQueries
{
    private readonly AppDbContext _context;

    public StoreCalendarQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<AvailableSlotDTO>> GetAvailableSlotsAsync(int storeId, int professionalId, int offeringId, DateOnly date, CancellationToken ct)
    {
        var schedule = await _context.ProfessionalSchedules
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProfessionalId == professionalId, ct);

        var calendar = schedule?.Calendars
            .FirstOrDefault(s => s.StoreId == storeId);

        if (calendar is null)
            return [];

        var ranges = calendar.ResolveTimeRanges(date);

        if (!ranges.Any())
            return [];

        var catalog = await _context.StoreCatalogs
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.StoreId == storeId, ct);

        if (catalog is null)
            return [];

        var offering = catalog.Offerings
            .FirstOrDefault(o => o.Id == offeringId);

        if (offering is null)
            return [];

        var duration = TimeSpan.FromMinutes(offering.Duration.Minutes);

        var dayStart = date.ToDateTime(TimeOnly.MinValue);
        var nextDay = dayStart.AddDays(1);

        var appointments = await _context.Appointments
            .Where(a =>
                a.StoreId == storeId &&
                a.ProfessionalId == professionalId &&
                a.StartAt.Value < nextDay &&
                a.EndAt.Value > dayStart)
            .OrderBy(a => a.StartAt.Value)
            .ToListAsync(ct);

        var slots = new List<AvailableSlotDTO>();

        foreach (var range in ranges)
        {
            var current = range.Start;

            while (current + duration <= range.End)
            {
                var end = current + duration;

                var slotStart = date.ToDateTime(TimeOnly.FromTimeSpan(current));
                var slotEnd = date.ToDateTime(TimeOnly.FromTimeSpan(end));

                var overlaps = appointments.Any(a =>
                    a.StartAt.Value < slotEnd &&
                    a.EndAt.Value > slotStart
                );

                if (!overlaps)
                {
                    slots.Add(new AvailableSlotDTO(slotStart, slotEnd));
                }

                current += TimeSpan.FromMinutes(Duration.SchedulingGranuleMinutes);
            }
        }

        return slots;
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> GetProfessionalCalendarAsync(int storeId, int professionalId, DateOnly from, DateOnly to, CancellationToken ct)
    {
        var schedule = await _context.ProfessionalSchedules
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.ProfessionalId == professionalId, ct);

        var calendar = schedule?.Calendars
            .FirstOrDefault(c => c.StoreId == storeId);

        if (calendar is null)
            return [];

        return BuildCalendar(calendar, from, to);
    }

    public async Task<IReadOnlyCollection<CalendarDayDTO>> GetStoreCalendarAsync(int storeId, DateOnly from, DateOnly to, CancellationToken ct)
    {
        var calendar = await _context.StoreCalendars
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.StoreId == storeId, ct);

        if (calendar is null)
            return [];

        return BuildCalendar(calendar, from, to);
    }

    private static IReadOnlyCollection<CalendarDayDTO> BuildCalendar(
        BaseCalendar calendar,
        DateOnly from,
        DateOnly to
    )
    {
        var days = new List<CalendarDayDTO>();

        for (var date = from; date <= to; date = date.AddDays(1))
        {
            var ranges = calendar.ResolveTimeRanges(date);

            days.Add(
                new CalendarDayDTO(
                    date,
                    ranges.Select(r => new TimeRangeDTO(r.Start, r.End)).ToList()
                )
            );
        }

        return days;
    }
}
