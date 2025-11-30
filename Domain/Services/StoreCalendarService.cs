using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class StoreCalendarService
{
    private readonly IStoreScheduleRepository _storeScheduleRepository;
    private readonly IStoreStaffRepository _storeStaffRepository;

    public StoreCalendarService(IStoreScheduleRepository storeScheduleRepository, IStoreStaffRepository storeStaffRepository)
    {
        _storeScheduleRepository = storeScheduleRepository;
        _storeStaffRepository = storeStaffRepository;
    }

    public async Task<StoreSchedule> AddStoreScheduleAsync(int ownerId, int storeId, DayOfWeek day, TimeSpan? openTime = null, TimeSpan? closeTime = null)
    {
        var storeScheduleManager = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        var schedule = storeScheduleManager.AddStoreSchedule(day, openTime, closeTime);
        await _storeScheduleRepository.SaveAsync(storeScheduleManager);
        return schedule;
    }

    public async Task RemoveStoreSchedule(int ownerId, int storeId, int scheduleId)
    {
        var storeScheduleManager = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        storeScheduleManager.RemoveStoreSchedule(scheduleId);
        await _storeScheduleRepository.SaveAsync(storeScheduleManager);
    }

    public async Task<StoreScheduleSpecial> AddStoreScheduleSpecial(int ownerId, int storeId, DateTime date, TimeSpan? openTime = null, TimeSpan? closeTime = null, string? reason = null)
    {
        var storeScheduleManager = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        var exception = storeScheduleManager.AddStoreScheduleSpecial(date, openTime, closeTime);
        await _storeScheduleRepository.SaveAsync(storeScheduleManager);
        return exception;
    }

    public async Task RemoveStoreScheduleSpecial(int ownerId, int storeId, int exceptionId)
    {
        var storeScheduleManager = await _storeScheduleRepository.GetByStoreAsync(storeId);
        var storeStaffManager = await _storeStaffRepository.GetByStoreAsync(storeId);

        if (!storeStaffManager.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the working schedule.");
        }

        storeScheduleManager.RemoveStoreScheduleSpecial(exceptionId);
        await _storeScheduleRepository.SaveAsync(storeScheduleManager);
    }
}