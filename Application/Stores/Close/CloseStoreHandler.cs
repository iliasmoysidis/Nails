using Domain.Stores.Services;
using Domain.Common;
using MediatR;

namespace Application.Stores.Close;

public sealed class CloseStoreHandler
    : IRequestHandler<CloseStoreCommand>
{
    private readonly CloseStoreContext _ctx;
    private readonly IClock _clock;

    public CloseStoreHandler(
        CloseStoreContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(CloseStoreCommand command, CancellationToken ct)
    {
        _ctx.StoreClosure.Close(_clock);

        return Task.CompletedTask;
    }
}
