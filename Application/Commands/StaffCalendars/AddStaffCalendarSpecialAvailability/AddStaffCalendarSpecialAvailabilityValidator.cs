using Application.Abstractions.Validation;
using Application.Guards;
using Domain.ValueObjects.Calendar;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityValidator
    : IRequestValidator<AddStaffCalendarSpecialAvailabilityCommand>
{
    private readonly ValidationGuard _val;
    private readonly AddStaffCalendarSpecialAvailabilityContext _ctx;

    public AddStaffCalendarSpecialAvailabilityValidator(
        ValidationGuard val,
        AddStaffCalendarSpecialAvailabilityContext ctx)
    {
        _val = val;
        _ctx = ctx;
    }

    public Task ValidateAsync(
        AddStaffCalendarSpecialAvailabilityCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _val.EnsureExceptionFitsStoreHours(_ctx.StoreCalendar, exception);

        return Task.CompletedTask;
    }
}