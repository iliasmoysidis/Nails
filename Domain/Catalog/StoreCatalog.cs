using Domain.Catalog.Entities;
using Domain.Catalog.ValueObjects;
using Domain.Common.Exceptions;
using Domain.Common.ValueObjects;

namespace Domain.Catalog;

public class StoreCatalog
{
    public int StoreId { get; }

    private readonly List<Offering> _offerings = new();
    public IReadOnlyCollection<Offering> Offerings => _offerings;

    private StoreCatalog() { }

    public StoreCatalog(int storeId)
    {
        StoreId = storeId;
    }

    public Offering AddOffering(
        OfferingName name,
        Money price,
        Duration duration,
        Description description)
    {
        EnsureNameIsUnique(name);

        var offering = Offering.Create(
            StoreId,
            name,
            price,
            duration,
            description);

        _offerings.Add(offering);

        return offering;
    }

    public void RemoveOffering(int offeringId)
    {
        var offering = GetOffering(offeringId);
        _offerings.Remove(offering);
    }

    public void Clear()
        => _offerings.Clear();

    public void UpdateOffering(
        int offeringId,
        OfferingName? name = null,
        Money? price = null,
        Duration? duration = null,
        Description? description = null)
    {
        var offering = GetOffering(offeringId);

        if (name != null)
            EnsureNameIsUnique(name, offeringId);

        offering.UpdateDetails(
            name,
            price,
            duration,
            description);
    }

    public Offering GetOffering(int offeringId)
        => _offerings.FirstOrDefault(o => o.Id == offeringId)
           ?? throw new NotFoundException("Offering not found.");

    private void EnsureNameIsUnique(OfferingName name, int? ignoreId = null)
    {
        if (_offerings.Any(o => o.Name == name && o.Id != ignoreId))
            throw new InvariantException("Offering name must be unique.");
    }

    public static StoreCatalog Rehydrate(
        int storeId,
        IEnumerable<Offering> offerings
    )
    {
        var catalog = new StoreCatalog(storeId);

        foreach (var offering in offerings)
        {
            if (offering.StoreId != storeId)
                throw new InvariantException("Offering does not belong to this store.");

            catalog._offerings.Add(offering);
        }

        return catalog;
    }
}
