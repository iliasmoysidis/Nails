using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Time;

namespace Application.Services;

public sealed class AppointmentSchedulingService : IAppointmentAvailabilityService
{
    private readonly AppointmentAvailabilityService _service;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public AppointmentSchedulingService(
        AppointmentAvailabilityService service,
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentRepository appointmentRepo
    )
    {
        _service = service;
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
        _appointmentRepo = appointmentRepo;
    }

    public async Task EnsureAvailableAsync(
        int storeId,
        int professionalId,
        UtcDateTime startAt,
        UtcDateTime endAt,
        int? ignoreAppointmentId,
        CancellationToken ct)
    {
        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(storeId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(storeId, professionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(professionalId, ct);

        try
        {
            _service.EnsureAppointmentAvailable(
                storeCalendar,
                staffCalendar,
                appointments,
                startAt,
                endAt,
                ignoreAppointmentId
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }

    }
}