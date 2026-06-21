using MediatR;

namespace Application.Catalog.Remove;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(Command command, CancellationToken ct)
    {
        _ctx.StoreOfferingRemoval.RemoveOffering(command.OfferingId);

        return Task.CompletedTask;
    }
}
