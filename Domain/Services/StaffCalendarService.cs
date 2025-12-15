using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public class StaffCalendarService
{
    private readonly IStaffCalendarRepository _staffCalendarRepository;
    private readonly IStaffRepository _staffRepository;

    public StaffCalendarService(IStaffCalendarRepository staffCalendarRepository, IStaffRepository staffRepository)
    {
        _staffCalendarRepository = staffCalendarRepository;
        _staffRepository = staffRepository;
    }

    public async Task SetWorkingDayAsync(int ownerId, int storeId, int professionalId, WorkingDay workingDay)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId, professionalId);

        await EnsureNoCrossStoreRecurringConflict(storeId, professionalId, workingDay);

        calendar.SetWorkingDay(workingDay);

        await _staffCalendarRepository.SaveAsync(calendar);
    }

    public async Task SetDayOffAsync(int ownerId, int storeId, int professionalId, DayOfWeek day)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId, professionalId);

        calendar.SetDayOff(day);

        await _staffCalendarRepository.SaveAsync(calendar);
    }

    public async Task AddExceptionAsync(int ownerId, int storeId, int professionalId, CalendarException exception)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId, professionalId);

        await EnsureNoCrossStoreDateSpecificConflict(storeId, professionalId, exception);

        calendar.AddException(exception);

        await _staffCalendarRepository.SaveAsync(calendar);
    }

    public async Task RemoveExceptionAsync(int ownerId, int storeId, int professionalId, DateOnly date)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId, professionalId);

        calendar.RemoveException(date);

        await _staffCalendarRepository.SaveAsync(calendar);
    }

    private async Task<StaffCalendar> GetAuthorizedCalendarAsync(int ownerId, int storeId, int professionalId)
    {
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId)) throw new DomainException("Only an owner can modify staff schedules.");

        if (!staff.IsStaff(professionalId)) throw new DomainException("Employee not found.");

        return await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
    }

    private async Task EnsureNoCrossStoreRecurringConflict(int storeId, int professionalId, WorkingDay workingDay)
    {
        if (workingDay.IsDayOff) return;

        var calendars = await _staffCalendarRepository.GetAllByProfessionalAsync(professionalId);

        foreach (var calendar in calendars)
        {
            if (calendar.StoreId == storeId) continue;

            if (!calendar.TryGetWorkingDay(workingDay.Day, out var other)) continue;

            if (other.IsDayOff) continue;

            if (TimeRange.AnyOverlap(workingDay.TimeRanges, other.TimeRanges))
            {
                throw new DomainException($"Professional has a conflicting schedule at another store on {workingDay.Day}");
            }
        }
    }

    private async Task EnsureNoCrossStoreDateSpecificConflict(int storeId, int professionalId, CalendarException exception)
    {
        if (exception.IsDayOff) return;

        var calendars = await _staffCalendarRepository.GetAllByProfessionalAsync(professionalId);
        var dayOfWeek = exception.Date.DayOfWeek;

        foreach (var calendar in calendars)
        {
            if (calendar.StoreId == storeId) continue;

            if (calendar.TryGetException(exception.Date, out var other))
            {
                if (!other.IsDayOff && TimeRange.AnyOverlap(exception.TimeRanges, other.TimeRanges))
                {
                    throw new DomainException($"Professional has a conflicting exception at another store on {exception.Date}.");
                }

                continue;
            }

            if (!calendar.TryGetWorkingDay(dayOfWeek, out var workingDay)) continue;

            if (workingDay.IsDayOff) continue;

            if (TimeRange.AnyOverlap(exception.TimeRanges, workingDay.TimeRanges))
            {
                throw new DomainException($"Professional has a conflicting schedule at another store on {exception.Date}.");
            }
        }
    }
}