using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Features.Offerings.Create;

public sealed class Handler
    : IRequestHandler<Command, int>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task<int> Handle(Command command, CancellationToken ct)
    {
        var offering = _ctx.Catalog.AddOffering(
            name: OfferingName.Create(command.Name),
            price: Money.Create(command.Price, command.Currency),
            duration: Duration.FromMinutes(command.DurationMinutes),
            description: Description.From(command.Description)
        );

        return Task.FromResult(offering.Id);
    }
}