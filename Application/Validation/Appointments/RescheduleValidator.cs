using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Time;

namespace Application.Validation.Appointments;

public sealed class RescheduleValidator : IRescheduleValidator
{
    private readonly AppointmentAvailabilityService _service;

    public RescheduleValidator(
        AppointmentAvailabilityService service
    )
    {
        _service = service;
    }

    public void EnsureAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar,
        Appointment appointment,
        IReadOnlyCollection<Appointment> appointments,
        UtcDateTime newStartAt
    )
    {
        var newEndAt = newStartAt.Add(appointment.Duration.Value);

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