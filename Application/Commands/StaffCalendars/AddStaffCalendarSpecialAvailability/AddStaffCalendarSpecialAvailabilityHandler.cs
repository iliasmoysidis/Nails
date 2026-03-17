using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityHandler
    : IRequestHandler<AddStaffCalendarSpecialAvailabilityCommand>
{
    private readonly AddStaffCalendarSpecialAvailabilityContext _ctx;

    public AddStaffCalendarSpecialAvailabilityHandler(
        AddStaffCalendarSpecialAvailabilityContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddStaffCalendarSpecialAvailabilityCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges.Select(r => new TimeRange(r.Start, r.End));

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _ctx.StaffCalendar.AddException(exception);

        return Task.CompletedTask;
    }
}