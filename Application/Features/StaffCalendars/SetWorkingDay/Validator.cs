using Application.Abstractions.Validation;
using Application.Guards;
using Domain.ValueObjects.Calendar;

namespace Application.Features.StaffCalendars.SetWorkingDay;

public sealed class Validator
    : IRequestValidator<Command>
{
    private readonly ValidationGuard _val;
    private readonly Context _ctx;

    public Validator(
        ValidationGuard val,
        Context ctx
    )
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(
        Command command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));
        var workingDay = WorkingDay.WithRanges(command.Day, ranges);

        _val.EnsureWorkingDayFitsStoreHours(_ctx.StoreCalendar, workingDay);

        return Task.CompletedTask;
    }
}