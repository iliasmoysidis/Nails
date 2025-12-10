using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class StaffCalendarService
{
    private readonly IStaffCalendarRepository _staffCalendarRepository;
    private readonly IStoreCalendarRepository _storeCalendarRepository;
    private readonly IStaffRepository _staffRepository;

    public StaffCalendarService(IStaffCalendarRepository staffCalendarRepository, IStoreCalendarRepository storeCalendarRepository, IStaffRepository staffRepository)
    {
        _staffCalendarRepository = staffCalendarRepository;
        _storeCalendarRepository = storeCalendarRepository;
        _staffRepository = staffRepository;
    }

    public async Task<EmployeeSchedule> AddStaffSchedule(int ownerId, int storeId, int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        var staffCalendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var storeCalendar = await _storeCalendarRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the schedule of an employee");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("Employee not found.");
        }

        if (!storeCalendar.IsWithinStoreHours(day, startTime, endTime))
        {
            throw new DomainException("Schedule is not within store hours");
        }

        var allCalendars = await _staffCalendarRepository.GetAllByProfessionalAsync(professionalId);

        if (HasCrossStoreConflict(allCalendars, storeId, day, startTime, endTime))
        {
            throw new DomainException("Professional is already schedule at another store for this time.");
        }

        var schedule = staffCalendar.AddStaffSchedule(day, startTime, endTime);
        await _staffCalendarRepository.SaveAsync(staffCalendar);
        return schedule;
    }

    public async Task RemoveStaffSchedule(int ownerId, int storeId, int professionalId, int scheduleId)
    {
        var staffCalendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the schedule of an employee");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("Employee not found.");
        }

        staffCalendar.RemoveStaffSchedule(scheduleId);
        await _staffCalendarRepository.SaveAsync(staffCalendar);
    }

    public async Task<EmployeeScheduleSpecial> AddStaffException(int ownerId, int storeId, int professionalId, DateTime date, TimeSpan? startTime = null, TimeSpan? endTime = null, string? reason = null)
    {
        var staffCalendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the schedule of an employee");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("Employee not found.");
        }

        if (startTime.HasValue && endTime.HasValue)
        {
            var allCalendars = await _staffCalendarRepository.GetAllByProfessionalAsync(professionalId);
            if (HasCrossStoreExceptionConflict(allCalendars, storeId, date, startTime, endTime))
            {
                throw new DomainException("Professional already has a conflicting schedule at another store.");
            }
        }

        var exception = staffCalendar.AddStaffException(date, startTime, endTime, reason);
        await _staffCalendarRepository.SaveAsync(staffCalendar);
        return exception;
    }

    public async Task RemoveStaffException(int ownerId, int storeId, int professionalId, int exceptionId)
    {
        var staffCalendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the schedule of an employee");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("Employee not found.");
        }

        staffCalendar.RemoveStaffException(exceptionId);
        await _staffCalendarRepository.SaveAsync(staffCalendar);
    }

    private bool HasCrossStoreConflict(IReadOnlyCollection<StaffCalendar> calendars, int storeId, DayOfWeek day, TimeSpan? startTime, TimeSpan? endTime)
    {
        foreach (var calendar in calendars)
        {
            if (calendar.StoreId == storeId)
            {
                continue;
            }

            var schedules = calendar.Schedules.Where(s => s.Day == day && s.IsWorking).ToList();

            foreach (var s in schedules)
            {
                if (!s.StartTime.HasValue || !s.EndTime.HasValue)
                {
                    continue;
                }

                bool isOverlapping = startTime < s.EndTime && endTime > s.StartTime;

                if (isOverlapping)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool HasCrossStoreExceptionConflict(IReadOnlyCollection<StaffCalendar> calendars, int currentStoreId, DateTime date, TimeSpan? newStart, TimeSpan? newEnd)
    {
        var day = date.DayOfWeek;

        foreach (var calendar in calendars)
        {
            if (calendar.StoreId == currentStoreId)
            {
                continue;
            }

            var exceptions = calendar.Exceptions.Where(e => e.Date.Date == date.Date).ToList();
            foreach (var exception in exceptions)
            {
                if (exception.IsDayOff)
                {
                    continue;
                }

                bool isOverlapping = newStart < exception.EndTime && newEnd > exception.StartTime;

                if (isOverlapping)
                {
                    return true;
                }
            }

            if (!exceptions.Any())
            {
                var schedules = calendar.Schedules.Where(s => s.Day == day && s.IsWorking).ToList();

                foreach (var s in schedules)
                {
                    bool isOverlapping = newStart < s.EndTime && newEnd > s.StartTime;

                    if (isOverlapping)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}