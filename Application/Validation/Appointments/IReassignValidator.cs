using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;

namespace Application.Validation.Appointments;

public sealed class ReassignValidator : IReassignValidator
{
    private readonly AppointmentAvailabilityService _service;

    public ReassignValidator(
        AppointmentAvailabilityService service
    )
    {
        _service = service;
    }

    public void EnsureAvailable(
        Appointment appointment,
        IReadOnlyCollection<Appointment> appointments,
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar
    )
    {
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