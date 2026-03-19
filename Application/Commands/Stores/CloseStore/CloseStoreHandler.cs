using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Stores;

public sealed class CloseStoreHandler
    : IRequestHandler<CloseStoreCommand>
{
    private readonly CloseStoreContext _ctx;
    private readonly IClock _clock;

    public CloseStoreHandler(
        CloseStoreContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(CloseStoreCommand command, CancellationToken ct)
    {
        _ctx.Store.Close(_clock);

        return Task.CompletedTask;
    }
}