using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Commands.Offerings;

public sealed class CreateOfferingHandler
    : IRequestHandler<CreateOfferingCommand, int>
{
    private readonly CreateOfferingContext _ctx;
    private readonly IClock _clock;

    public CreateOfferingHandler(
        CreateOfferingContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task<int> Handle(CreateOfferingCommand command, CancellationToken ct)
    {
        var offering = _ctx.Catalog.AddOffering(
            name: OfferingName.Create(command.Name),
            price: Money.Create(command.Price, command.Currency),
            duration: Duration.FromMinutes(command.DurationMinutes),
            description: Description.From(command.Description),
            clock: _clock
        );

        return Task.FromResult(offering.Id);
    }
}