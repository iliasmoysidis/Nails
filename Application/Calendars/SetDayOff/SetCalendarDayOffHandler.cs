using MediatR;

namespace Application.Calendars.SetDayOff;

public sealed class SetCalendarDayOffHandler
    : IRequestHandler<SetCalendarDayOffCommand>
{
    private readonly SetCalendarDayOffContext _ctx;

    public SetCalendarDayOffHandler(SetCalendarDayOffContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetCalendarDayOffCommand command,
        CancellationToken ct)
    {
        _ctx.StoreAvailability.CloseDay(command.Day);

        return Task.CompletedTask;
    }
}
