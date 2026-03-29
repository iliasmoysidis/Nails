using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StoreCalendars.AddHoliday;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct
    )
    {
        var holiday = CalendarException.DayOff(command.Date);

        _ctx.StoreCalendar.SetException(holiday);

        return Task.CompletedTask;
    }
}