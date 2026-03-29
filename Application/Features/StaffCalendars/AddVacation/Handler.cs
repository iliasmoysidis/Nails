using Domain.ValueObjects.Calendar;
using MediatR;

namespace Application.Features.StaffCalendars.AddVacation;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(
        Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct)
    {
        var holiday = CalendarException.DayOff(command.Date);

        _ctx.StaffCalendar.AddException(holiday);

        return Task.CompletedTask;
    }
}