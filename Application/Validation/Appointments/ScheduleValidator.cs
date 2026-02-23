using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.Appointments;
using Application.Commands.Appointments;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Time;

namespace Application.Validation.Appointments;

public sealed class ScheduleValidator : IScheduleValidator
{
    private readonly AppointmentAvailabilityService _service;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public ScheduleValidator(
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

    public async Task EnsureAvailableAsync(ScheduleCommand command, CancellationToken ct)
    {
        var startAt = UtcDateTime.FromUtc(command.StartAt);
        var duration = Duration.FromMinutes(command.Duration);
        var endAt = startAt.Add(duration.Value);

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(command.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);

        try
        {
            _service.EnsureAppointmentAvailable(
                storeCalendar,
                staffCalendar,
                appointments,
                startAt,
                endAt,
                null
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }
}