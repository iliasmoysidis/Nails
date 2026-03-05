using Application.Abstractions.Validation.Appointments;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Time;

namespace Application.Validation.Appointments;

public sealed class ScheduleValidator : IScheduleValidator
{
    private readonly AppointmentAvailabilityService _service;

    public ScheduleValidator(
        AppointmentAvailabilityService service
    )
    {
        _service = service;
    }

    public void EnsureAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar, IReadOnlyCollection<Appointment> appointments, UtcDateTime startAt,
        UtcDateTime endAt
        )
    {
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