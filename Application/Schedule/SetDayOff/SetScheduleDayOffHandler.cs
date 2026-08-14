using MediatR;

namespace Application.Schedule.SetDayOff;

public sealed class SetScheduleDayOffHandler
    : IRequestHandler<SetScheduleDayOffCommand>
{
    private readonly SetScheduleDayOffContext _ctx;

    public SetScheduleDayOffHandler(SetScheduleDayOffContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        SetScheduleDayOffCommand command,
        CancellationToken ct)
    {
        _ctx.ProfessionalAvailability.SetDayOff(command.Day);

        return Task.CompletedTask;
    }
}
