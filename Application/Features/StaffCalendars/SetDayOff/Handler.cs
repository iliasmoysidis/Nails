using MediatR;

namespace Application.Features.StaffCalendars.SetDayOff;

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
        CancellationToken ct)
    {
        _ctx.StaffCalendar.SetDayOff(command.Day);

        return Task.CompletedTask;
    }
}