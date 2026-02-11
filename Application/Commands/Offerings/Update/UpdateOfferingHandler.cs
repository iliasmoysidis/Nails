using Application.Abstractions.Policies.Offerings;
using Application.Abstractions.Repositories;
using Application.Abstractions.UnitOfWork;
using Application.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingHandler
{
    private readonly IManageOfferingPolicy _policy;
    private readonly IStoreCatalogRepository _repo;
    private readonly IClock _clock;
    private readonly IUnitOfWork _uow;

    public UpdateOfferingHandler(
        IManageOfferingPolicy policy,
        IStoreCatalogRepository repo,
        IClock clock,
        IUnitOfWork uow
    )
    {
        _policy = policy;
        _repo = repo;
        _clock = clock;
        _uow = uow;
    }

    public async Task Handle(UpdateOfferingCommand command, CancellationToken ct)
    {
        await _policy.EnsureCanManageAsync(command.StoreId, ct);

        var catalog = await _repo.GetByStoreIdAsync(command.StoreId, ct)
            ?? throw new ApplicationLayerNotFoundException($"Store catalog not found for store {command.StoreId}.");

        var offering = catalog.GetOfferingOrThrow(command.OfferingId);

        catalog.UpdateOffering(
            offeringId: command.OfferingId,
            clock: _clock,
            name: ToName(command.Name),
            price: ToPrice(command.Price, offering.Price.Currency),
            duration: ToDuration(command.DurationMinutes),
            description: ToDescription(command.Description)
            );

        await _uow.SaveChangesAsync(ct);
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