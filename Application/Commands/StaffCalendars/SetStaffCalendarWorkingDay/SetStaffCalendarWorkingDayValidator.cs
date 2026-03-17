using Application.Abstractions.Validation;
using Application.Guards;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarWorkingDayValidator
    : IRequestValidator<SetStaffCalendarWorkingDayCommand>
{
    private readonly ValidationGuard _val;
    private readonly SetStaffCalendarWorkingDayContext _ctx;

    public SetStaffCalendarWorkingDayValidator(
        ValidationGuard val,
        SetStaffCalendarWorkingDayContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(
        SetStaffCalendarWorkingDayCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));
        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _val.EnsureWorkingDayFitsStoreHours(_ctx.StoreCalendar, workingDay);

        return Task.CompletedTask;
    }
}