using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Calendar;
using Domain.ValueObjects.Time;

namespace Application.Guards;

public sealed class ValidationGuard
{
    private readonly AppointmentAvailabilityService _availabilityService;
    private readonly SchedulingService _schedulingService;

    public ValidationGuard(
        AppointmentAvailabilityService appointmentAvailabilityService,
        SchedulingService schedulingService
    )
    {
        _availabilityService = appointmentAvailabilityService;
        _schedulingService = schedulingService;
    }

    public void EnsureWorkingDayFitsStoreHours(StoreCalendar calendar, WorkingDay workingDay)
    {
        try
        {
            _schedulingService.EnsureWorkingDayFitsStoreHours(calendar, workingDay);
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }

    public void EnsureExceptionFitsStoreHours(StoreCalendar calendar, CalendarException exception)
    {
        try
        {
            _schedulingService.EnsureExceptionFitsStoreHours(calendar, exception);
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }

    public void EnsureAppointmentAvailable(
        StoreCalendar storeCalendar,
        StaffCalendar staffCalendar, IReadOnlyCollection<Appointment> appointments, UtcDateTime startAt,
        UtcDateTime endAt
        )
    {
        try
        {
            _availabilityService.EnsureAppointmentAvailable(
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