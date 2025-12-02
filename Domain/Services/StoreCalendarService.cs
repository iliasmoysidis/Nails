using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class StoreCalendarService
{
    private readonly IStoreCalendarRepository _storeScheduleRepository;
    private readonly IStaffRepository _storeStaffRepository;

    public StoreCalendarService(IStoreCalendarRepository storeScheduleRepository, IStaffRepository storeStaffRepository)
    {
        _storeScheduleRepository = storeScheduleRepository;
        _storeStaffRepository = storeStaffRepository;
    }

    public async Task<StoreSchedule> AddStoreScheduleAsync(int ownerId, int storeId, DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        var StoreCalendar = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        var schedule = StoreCalendar.AddStoreSchedule(day, openTime, closeTime);
        await _storeScheduleRepository.SaveAsync(StoreCalendar);
        return schedule;
    }

    public async Task RemoveStoreSchedule(int ownerId, int storeId, int scheduleId)
    {
        var StoreCalendar = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        StoreCalendar.RemoveStoreSchedule(scheduleId);
        await _storeScheduleRepository.SaveAsync(StoreCalendar);
    }

    public async Task<StoreScheduleSpecial> AddStoreScheduleSpecial(int ownerId, int storeId, DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        var StoreCalendar = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        var exception = StoreCalendar.AddStoreScheduleSpecial(date, openTime, closeTime, reason);
        await _storeScheduleRepository.SaveAsync(StoreCalendar);
        return exception;
    }

    public async Task RemoveStoreScheduleSpecial(int ownerId, int storeId, int exceptionId)
    {
        var StoreCalendar = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        StoreCalendar.RemoveStoreScheduleSpecial(exceptionId);
        await _storeScheduleRepository.SaveAsync(StoreCalendar);
    }
}