using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.Appointments;
using Application.Commands.Appointments;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;

namespace Application.Validation.Appointments;

public sealed class ReassignValidator : IReassignValidator
{
    private readonly AppointmentAvailabilityService _service;
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    public ReassignValidator(
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

    public async Task EnsureAvailableAsync(ReassignCommand command, CancellationToken ct)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId, ct)
            ?? throw new ApplicationLayerNotFoundException("Appointment not found.");

        var storeCalendar = await _storeCalendarRepo.GetByStoreIdAsync(appointment.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        var staffCalendar = await _staffCalendarRepo.GetAsync(appointment.StoreId, command.ProfessionalId, ct)
            ?? throw new ApplicationLayerValidationException("Professional calendar not found.");

        var appointments = await _appointmentRepo.GetByProfessionalIdAsync(command.ProfessionalId, ct);

        try
        {
            _service.EnsureAppointmentAvailable(
                storeCalendar,
                staffCalendar,
                appointments,
                appointment.StartAt,
                appointment.EndAt,
                null
            );
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }
}