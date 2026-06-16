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
        _ctx.StoreAvailability.AddHoliday(command.Date);

        return Task.CompletedTask;
    }
}
