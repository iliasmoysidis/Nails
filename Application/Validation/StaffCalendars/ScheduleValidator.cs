using Application.Abstractions.Validation.StaffCalendars;
using Application.Exceptions;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Calendar;

namespace Application.Validation.StaffCalendars;

public sealed class ScheduleValidator : IScheduleValidator
{
    private readonly ISchedulingService _service;

    public ScheduleValidator(
        ISchedulingService service
    )
    {
        _service = service;
    }

    public void EnsureWorkingDayFitsStoreHours(StoreCalendar calendar, WorkingDay workingDay)
    {
        try
        {
            _service.EnsureWorkingDayFitsStoreHours(calendar, workingDay);
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
            _service.EnsureExceptionFitsStoreHours(calendar, exception);
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }
}