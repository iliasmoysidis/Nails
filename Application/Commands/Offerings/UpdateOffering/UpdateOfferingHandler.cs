using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;
using MediatR;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingHandler
    : IRequestHandler<UpdateOfferingCommand>
{
    private readonly UpdateOfferingContext _ctx;
    private readonly IClock _clock;

    public UpdateOfferingHandler(
        UpdateOfferingContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(UpdateOfferingCommand command, CancellationToken ct)
    {
        var offering = _ctx.Catalog.GetOffering(command.OfferingId);

        _ctx.Catalog.UpdateOffering(
            offeringId: command.OfferingId,
            clock: _clock,
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