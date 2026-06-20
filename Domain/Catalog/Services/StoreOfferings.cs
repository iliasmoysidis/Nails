using Domain.Catalog.Entities;
using Domain.Stores;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;
using Domain.Catalog.ValueObjects;

namespace Domain.Catalog.Services;

public sealed class StoreOfferings
{
    private readonly Store _store;
    private readonly StoreCatalog _catalog;

    public StoreOfferings(Store store, StoreCatalog catalog)
    {
        ValidateComposition(store, catalog);

        _store = store;
        _catalog = catalog;
    }

    public Offering AddOffering(
        string name,
        decimal price,
        string currency,
        int durationMinutes,
        string? description
    )
    {
        _store.EnsureOpen();

        return _catalog.AddOffering(
            name: OfferingName.Create(name),
            price: Money.Create(price, currency),
            duration: Duration.FromMinutes(durationMinutes),
            description: Description.From(description)
        );
    }

    public void UpdateOffering(int offeringId, string? name, decimal? price, int? duration, string? description)
    {
        _store.EnsureOpen();

        var offering = _catalog.GetOffering(offeringId);

        _catalog.UpdateOffering(
            offeringId: offering.Id,
            name: ToName(name),
            price: ToPrice(price, offering.Price.Currency),
            duration: ToDuration(duration),
            description: ToDescription(description)
        );
    }

    private static void ValidateComposition(Store store, StoreCatalog catalog)
    {
        if (store.Id != catalog.StoreId)
            throw new InvariantException("Catalog does not belong to store.");
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
