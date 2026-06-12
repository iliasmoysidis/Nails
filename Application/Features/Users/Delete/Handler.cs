using Domain.Interfaces;
using MediatR;

namespace Application.Features.Users.Delete;

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

    public Task Handle(Command command, CancellationToken ct)
    {
        _ctx.UserDeletion.Delete(_clock);

        return Task.CompletedTask;
    }
}
