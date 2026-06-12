using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Features.Offerings.Update;

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
        var offering = _ctx.Catalog.GetOffering(command.OfferingId);

        _ctx.Catalog.UpdateOffering(
            offeringId: offering.Id,
            name: ToName(command.Name),
            price: ToPrice(command.Price, offering.Price.Currency),
            duration: ToDuration(command.DurationMinutes),
            description: ToDescription(command.Description)
        );

        return Task.CompletedTask;
    }

    private static OfferingName? ToName(string? value)
        => value is null ? null : OfferingName.Create(value);

    private static Money? ToPrice(decimal? value, string currency)
        => value is null ? null : Money.Create(value.Value, currency);

    private static Duration? ToDuration(int? minutes)
        => minutes is null ? null : Duration.FromMinutes(minutes.Value);

    private static Description? ToDescription(string? value)
        => value is null ? null : Description.From(value);
}
