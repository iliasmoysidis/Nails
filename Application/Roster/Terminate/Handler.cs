using Domain.Common;
using MediatR;

namespace Application.Roster.Terminate;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;
    private readonly IClock _clock;

    public Handler(Context ctx, IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(
        Command command,
        CancellationToken ct
    )
    {
        _ctx.EmploymentTermination.Terminate(_clock);

        return Task.CompletedTask;
    }
}
