using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.ValueObjects.Calendar;

namespace Domain.Services;

public class StoreCalendarService
{
    private readonly IStoreCalendarRepository _storeCalendarRepository;
    private readonly IStaffRepository _storeStaffRepository;

    public StoreCalendarService(IStoreCalendarRepository storeCalendarRepository, IStaffRepository storeStaffRepository)
    {
        _storeCalendarRepository = storeCalendarRepository;
        _storeStaffRepository = storeStaffRepository;
    }

    public async Task SetWorkingDayAsync(int ownerId, int storeId, WorkingDay workingDay)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId);

        calendar.SetWorkingDay(workingDay);

        await _storeCalendarRepository.SaveAsync(calendar);
    }

    public async Task SetDayOffAsync(int ownerId, int storeId, DayOfWeek day)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId);

        calendar.SetDayOff(day);

        await _storeCalendarRepository.SaveAsync(calendar);
    }

    public async Task AddExceptionAsync(int ownerId, int storeId, CalendarException exception)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId);

        calendar.AddException(exception);

        await _storeCalendarRepository.SaveAsync(calendar);
    }

    public async Task RemoveExceptionAsync(int ownerId, int storeId, DateOnly date)
    {
        var calendar = await GetAuthorizedCalendarAsync(ownerId, storeId);

        calendar.RemoveException(date);

        await _storeCalendarRepository.SaveAsync(calendar);
    }

    private async Task<StoreCalendar> GetAuthorizedCalendarAsync(int ownerId, int storeId)
    {
        var staff = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId)) throw new DomainException("Only an owner can modify the store calendar.");

        return await _storeCalendarRepository.GetByStoreAsync(storeId);
    }
}