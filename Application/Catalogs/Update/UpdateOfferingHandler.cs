using MediatR;

namespace Application.Catalogs.Update;

public sealed class UpdateOfferingHandler
    : IRequestHandler<UpdateOfferingCommand>
{
    private readonly UpdateOfferingContext _ctx;

    public UpdateOfferingHandler(UpdateOfferingContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(UpdateOfferingCommand command, CancellationToken ct)
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
