using Application.Abstractions.Validation;
using Application.Guards;
using Domain.ValueObjects.Calendar;

namespace Application.Features.StaffCalendars.AddSpecialAvailability;

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

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _val.EnsureExceptionFitsStoreHours(_ctx.StoreCalendar, exception);

        return Task.CompletedTask;
    }
}