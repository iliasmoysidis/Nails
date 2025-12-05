using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class StaffCalendarService
{
    private readonly IStaffCalendarRepository _staffCalendarRepository;
    private readonly IStoreCalendarRepository _storeCalendarRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IProfessionalCalendarRepository _professionalCalendarRepository;

    public StaffCalendarService(IStaffCalendarRepository staffCalendarRepository, IStoreCalendarRepository storeCalendarRepository, IStaffRepository staffRepository, IProfessionalCalendarRepository professionalCalendarRepository)
    {
        _staffCalendarRepository = staffCalendarRepository;
        _storeCalendarRepository = storeCalendarRepository;
        _staffRepository = staffRepository;
        _professionalCalendarRepository = professionalCalendarRepository;
    }

    public async Task<EmployeeSchedule> AddStaffSchedule(int ownerId, int storeId, int professionalId, DayOfWeek day, TimeSpan? startTime = null, TimeSpan? endTime = null)
    {
        var staffCalendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);
        var storeCalendar = await _storeCalendarRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);
        var professionalCalendar = await _professionalCalendarRepository.GetByProfessionalAsync(professionalId);

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

        // TODO: Check that professional is not working at other stores on time interval

        var schedule = staffCalendar.AddStaffSchedule(day, startTime, endTime);
        await _staffCalendarRepository.SaveAsync(staffCalendar);
        return schedule;
    }
}