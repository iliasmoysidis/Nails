using Application.Abstractions.Repositories;
using Application.Abstractions.Validation.StaffCalendars;
using Application.Commands.StaffCalendars;
using Application.Exceptions;
using Domain.ValueObjects.Calendar;

namespace Application.Validation.StaffCalendars;

public sealed class ScheduleValidator : IScheduleValidator
{
    private readonly IStoreCalendarRepository _repo;

    public ScheduleValidator(IStoreCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task EnsureFitsStoreHours(SetWorkingDayCommand command, CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        var calendar = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException("Store calendar not found.");

        if (workingDay.IsDayOff)
            return;

        foreach (var range in workingDay.TimeRanges)
        {
            if (!calendar.IsWhithinWeeklyStoreHours(workingDay.Day, range))
                throw new ApplicationLayerValidationException("Staff working hours must be within store opening hours.");
        }
    }
}