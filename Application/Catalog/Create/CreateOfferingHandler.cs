using MediatR;

namespace Application.Catalog.Create;

public sealed class CreateOfferingHandler
    : IRequestHandler<CreateOfferingCommand, int>
{
    private readonly CreateOfferingContext _ctx;

    public CreateOfferingHandler(CreateOfferingContext ctx)
    {
        _ctx = ctx;
    }

    public Task<int> Handle(CreateOfferingCommand command, CancellationToken ct)
    {
        var offering = _ctx.StoreOfferings.AddOffering(
            name: command.Name,
            price: command.Price,
            currency: command.Currency,
            durationMinutes: command.DurationMinutes,
            description: command.Description
        );

        return Task.FromResult(offering.Id);
    }
}
