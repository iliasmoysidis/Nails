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
        var offering = GetOfferingOrThrow(offeringId);
        offering.SoftDelete(clock);
    }

    public void UpdateOffering(int offeringId, IClock clock, OfferingName? name = null, Money? price = null, Duration? duration = null, Description? description = null)
    {
        var offering = GetOfferingOrThrow(offeringId);

        if (name is not null)
            EnsureNameIsUnique(name, offeringId);

        offering.UpdateDetails(
            clock: clock,
            name: name,
            price: price,
            duration: duration,
            description: description);
    }

    public Offering GetOfferingOrThrow(int offeringId)
        => _offerings.FirstOrDefault(s => s.Id == offeringId && !s.IsDeleted)
            ?? throw new NotFoundException("Offering not found.");

    public IReadOnlyCollection<Offering> GetActiveOfferings()
        => _offerings.Where(o => !o.IsDeleted).ToList().AsReadOnly();

    private void EnsureNameIsUnique(OfferingName name, int offeringId)
    {
        if (_offerings.Any(o => o.Id == offeringId && !o.IsDeleted && o.Name == name))
            throw new InvariantException("Offering name must be unique.");
    }
}