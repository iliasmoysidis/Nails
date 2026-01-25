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

    public Offering GetOfferingOrThrow(int offeringId)
        => _offerings.FirstOrDefault(s => s.Id == offeringId && !s.IsDeleted)
            ?? throw new NotFoundException("Offering not found.");

    public IReadOnlyCollection<Offering> GetActiveOfferings()
        => _offerings.Where(o => !o.IsDeleted).ToList().AsReadOnly();
}