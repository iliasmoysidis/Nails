using MediatR;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarDayOffHandler
    : IRequestHandler<SetStoreCalendarDayOffCommand>
{
    private readonly SetStoreCalendarDayOffContext _ctx;

    public SetStoreCalendarDayOffHandler(
        SetStoreCalendarDayOffContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetStoreCalendarDayOffCommand command,
        CancellationToken ct)
    {
        _ctx.StoreCalendar.SetDayOff(command.Day);

        return Task.CompletedTask;
    }
}