using MediatR;

namespace Application.Catalog.Update;

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
        _ctx.StoreOfferings.UpdateOffering(
            offeringId: command.OfferingId,
            name: command.Name,
            price: command.Price,
            duration: command.DurationMinutes,
            description: command.Description
        );

        return Task.CompletedTask;
    }
}
