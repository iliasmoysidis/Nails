using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed class AddStoreCalendarSpecialHoursHandler
    : IRequestHandler<AddStoreCalendarSpecialHoursCommand>
{
    private readonly AddStoreCalendarSpecialHoursContext _ctx;

    public AddStoreCalendarSpecialHoursHandler(
        AddStoreCalendarSpecialHoursContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        AddStoreCalendarSpecialHoursCommand command,
        CancellationToken ct)
    {
        var ranges = command.TimeRanges
            .Select(r => new TimeRange(r.Start, r.End))
            .ToList();

        var exception = CalendarException.PartialDay(command.Date, ranges);

        _ctx.StoreCalendar.AddException(exception);

        return Task.CompletedTask;
    }
}