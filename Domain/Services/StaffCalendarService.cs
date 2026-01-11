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
        var others = await GetOtherCalendars(professionalId, storeId);

        if (others.Any(c => c.ConflictsWithRecurring(workingDay)))
        {
            throw new DomainException($"Professional has a conflicting recurring schedule on {workingDay.Day}");
        }

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
        var others = await GetOtherCalendars(professionalId, storeId);

        if (others.Any(c => c.ConflictsWithDateSpecific(exception)))
        {
            throw new DomainException($"Professional has a conflicting schedule on {exception.Date}");
        }

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

        if (!staff.IsOwner(ownerId))
            throw new DomainException("Only an owner can modify staff schedules.");

        if (!staff.IsEmployee(professionalId))
            throw new DomainException("Employee not found.");

        return await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
    }

    private async Task<IEnumerable<StaffCalendar>> GetOtherCalendars(int professionalId, int storeId)
    {
        var calendars = await _staffCalendarRepository.GetAllByProfessionalAsync(professionalId);

        return calendars.Where(c => c.StoreId != storeId);
    }
}