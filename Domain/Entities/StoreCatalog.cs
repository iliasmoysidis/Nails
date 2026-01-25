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
    private readonly HashSet<ProfessionalOffering> _serviceOfferings = new();

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
        _serviceOfferings.RemoveWhere(o => o.OfferingId == offeringId);
    }

    public void AssignOffering(int professionalId, int offeringId)
    {
        GetOfferingOrThrow(offeringId);

        var assignment = new ProfessionalOffering(professionalId, offeringId);

        if (!_serviceOfferings.Add(assignment))
        {
            throw new InvariantException("Offering is already assigned to this professional.");
        }
    }

    public void UnassignOffering(int professionalId, int offeringId)
    {
        GetOfferingOrThrow(offeringId);

        if (!_serviceOfferings.Remove(new ProfessionalOffering(professionalId, offeringId)))
        {
            throw new InvariantException("Offering is not assigned to the professional.");
        }
    }

    public Offering GetOfferingOrThrow(int offeringId)
        => _offerings.FirstOrDefault(s => s.Id == offeringId && !s.IsDeleted)
            ?? throw new NotFoundException("Offering not found.");

    public Offering? GetOffering(int offeringId)
        => _offerings.FirstOrDefault(o => o.Id == offeringId && !o.IsDeleted);

    public bool OfferingExists(int offeringId)
        => _offerings.Any(o => o.Id == offeringId && !o.IsDeleted);

    public IReadOnlyCollection<Offering> GetActiveOfferings()
        => _offerings.Where(o => !o.IsDeleted).ToList().AsReadOnly();

    public bool IsOfferingProvidedByProfessional(int professionalId, int offeringId)
    {
        return _serviceOfferings.Any(so => so.ProfessionalId == professionalId && so.OfferingId == offeringId);
    }

    public IReadOnlyCollection<int> GetOfferingIdsForProfessional(int professionalId)
    {
        return _serviceOfferings
            .Where(so => so.ProfessionalId == professionalId)
            .Select(x => x.OfferingId)
            .ToList()
            .AsReadOnly();
    }
}