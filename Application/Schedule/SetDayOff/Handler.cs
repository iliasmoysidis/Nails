using MediatR;

namespace Application.Schedule.SetDayOff;

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
        _ctx.ProfessionalAvailability.SetDayOff(command.Day);

        return Task.CompletedTask;
    }
}
