using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.Appointments;
using Application.Commands.Appointments;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Time;

namespace Application.Validation.Appointments;

public sealed class RescheduleValidator : IRescheduleValidator
{
    private readonly AppointmentAvailabilityService _service;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public RescheduleValidator(
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

    public async Task EnsureAvailableAsync(RescheduleAppointmentCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var newStartAt = UtcDateTime.FromUtc(command.NewStartAt);
        var newEndAt = newStartAt.Add(appointment.Duration.Value);

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(appointment.StoreId, appointment.ProfessionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(appointment.ProfessionalId, ct);

        try
        {
            _service.EnsureAppointmentAvailable(
                storeCalendar,
                staffCalendar,
                appointments,
                newStartAt,
                newEndAt,
                appointment.Id
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }
}