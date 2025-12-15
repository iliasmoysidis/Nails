using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Repositories;

namespace Domain.Services;

public class AvailabilityService : IAvailabilityService
{
    private readonly IStoreCalendarRepository _storeCalendarRepository;
    private readonly IStaffCalendarRepository _staffCalendarRepository;

    public AvailabilityService(IStoreCalendarRepository storeCalendarRepository, IStaffCalendarRepository staffCalendarRepository)
    {
        _storeCalendarRepository = storeCalendarRepository;
        _staffCalendarRepository = staffCalendarRepository;
    }

    public async Task EnsureStoreIsOpenAsync(int storeId, DateTime startAt, DateTime endAt)
    {
        var calendar = await _storeCalendarRepository.GetByStoreAsync(storeId);

        if (!calendar.IsOpenAt(startAt) || !calendar.IsOpenAt(endAt.AddMinutes(-1)))
        {
            throw new DomainException("Store is closed at the requested time.");
        }
    }

    public async Task EnsureProfessionalIsAvailableAsync(int storeId, int professionalId, DateTime startAt, DateTime endAt)
    {
        var calendar = await _staffCalendarRepository.GetByStoreAndProfessionalAsync(storeId, professionalId);

        if (!calendar.IsProfessionalAvailable(startAt, endAt))
        {
            throw new DomainException("Professional is unavailable at the requested time.");
        }
    }
}