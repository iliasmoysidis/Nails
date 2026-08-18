using MediatR;

namespace Application.Catalogs.Remove;

public sealed class RemoveOfferingHandler
    : IRequestHandler<RemoveOfferingCommand>
{
    private readonly RemoveOfferingContext _ctx;

    public RemoveOfferingHandler(RemoveOfferingContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(RemoveOfferingCommand command, CancellationToken ct)
    {
        _ctx.StoreOfferingRemoval.RemoveOffering(command.OfferingId);

        return Task.CompletedTask;
    }
}
