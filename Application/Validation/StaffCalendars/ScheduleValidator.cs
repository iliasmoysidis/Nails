using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.StaffCalendars;
using Application.Commands.StaffCalendars;
using Application.Exceptions;
using Domain.Exceptions;
using Domain.Services;
using Domain.ValueObjects.Calendar;

namespace Application.Validation.StaffCalendars;

public sealed class ScheduleValidator : IScheduleValidator
{
    private readonly SchedulingService _service;
    private readonly IStoreCalendarRepository _repo;

    public ScheduleValidator(
        SchedulingService service,
        IStoreCalendarRepository repo
    )
    {
        _service = service;
        _repo = repo;
    }

    public async Task EnsureFitsStoreHours(SetWorkingDayCommand command, CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        var calendar = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        try
        {
            _service.EnsureWorkingDayFitsStoreHours(calendar, workingDay);
        }
        catch (DomainException ex)
        {
            throw new ApplicationLayerValidationException(ex.Message);
        }
    }

    public async Task EnsureExceptionFitsStoreHours(AddSpecialAvailabilityCommand command, CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        var calendar = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

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