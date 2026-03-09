using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class StoreCatalog
{
    public int StoreId { get; private set; }

    private readonly List<Offering> _offerings = new();
    public IReadOnlyCollection<Offering> Offerings
        => _offerings.Where(o => !o.IsDeleted).ToArray();

    private StoreCatalog() { }

    private StoreCatalog(int storeId)
    {
        StoreId = storeId;
    }

    public static StoreCatalog Create(int storeId)
    {
        return new(storeId);
    }

    public Offering AddOffering(
        OfferingName name,
        Money price,
        Duration duration,
        Description description,
        IClock clock
        )
    {
        EnsureNameIsUnique(name);

        var offering = Offering.Create(
            StoreId,
            name,
            price,
            duration,
            description,
            clock
            );

        _offerings.Add(offering);

        return offering;
    }

    public void RemoveOffering(int offeringId, IClock clock)
    {
        var offering = GetOffering(offeringId);

        offering.SoftDelete(clock);
    }

    public void Clear(IClock clock)
    {
        foreach (var offering in _offerings.Where(o => !o.IsDeleted))
        {
            offering.SoftDelete(clock);
        }
    }

    public void UpdateOffering(int offeringId, IClock clock, OfferingName? name = null, Money? price = null, Duration? duration = null, Description? description = null)
    {
        var offering = GetOffering(offeringId);

        if (name is not null)
            EnsureNameIsUnique(name, offeringId);

        offering.UpdateDetails(
            clock: clock,
            name: name,
            price: price,
            duration: duration,
            description: description);
    }

    public Offering GetOffering(int offeringId)
    {
        return _offerings.FirstOrDefault(o => o.Id == offeringId && !o.IsDeleted)
            ?? throw new NotFoundException("Offering not found.");
    }

    private void EnsureNameIsUnique(OfferingName name, int? offeringId = null)
    {
        if (_offerings.Any(o =>
            !o.IsDeleted &&
            o.Name == name &&
            (offeringId == null || o.Id != offeringId)))
            throw new InvariantException("Offering name must be unique.");
    }
}