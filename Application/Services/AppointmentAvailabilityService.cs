using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Application.Services;

public sealed class AppointmentAvailabilityService : IAppointmentAvailabilityService
{
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public AppointmentAvailabilityService(
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo,
        IAppointmentRepository appointmentRepo
    )
    {
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
            ?? throw new ApplicationLayerNotFoundException("Store calendar not configured.");

        if (!storeCalendar.IsWithinStoreHours(startAt.Date, new TimeRange(startAt.TimeOfDay, endAt.TimeOfDay)))
            throw new ApplicationLayerValidationException("Store is closed during the selected time.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(storeId, professionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not configured.");

        if (!staffCalendar.IsAvailable(startAt, endAt))
            throw new ApplicationLayerValidationException("Professional is not available during the selected time.");

        var appointments = await _appointmentRepo.GetActiveProfessionalAsync(professionalId, ct);

        var conflict = appointments.Any(a => a.Id != ignoreAppointmentId && a.ConflictsWith(startAt, endAt));

        if (conflict)
            throw new ApplicationLayerValidationException("Professional already has an appointment during this time.");
    }
}