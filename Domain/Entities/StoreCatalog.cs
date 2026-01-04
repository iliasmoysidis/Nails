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
    public IReadOnlyCollection<Offering> Offerings => _offerings.AsReadOnly();

    private readonly HashSet<ServiceOffering> _serviceOfferings = new();
    public IReadOnlyCollection<ServiceOffering> ServiceOfferings => _serviceOfferings;

    private StoreCatalog() { }

    public static StoreCatalog Create(int storeId)
    {
        return new StoreCatalog
        {
            StoreId = storeId
        };
    }

    public Offering AddOffering(OfferingName name, Money price, Duration duration, IClock clock, string? description = null)
    {
        var offering = Offering.Create(StoreId, name, price, duration, clock, description);

        _offerings.Add(offering);

        return offering;
    }

    public void RemoveOffering(int offeringId, IClock clock)
    {
        var offering = _offerings.FirstOrDefault(s => s.Id == offeringId && !s.IsDeleted)
            ?? throw new DomainException("Offering not found.");

        offering.SoftDelete(clock);

        _serviceOfferings.RemoveWhere(o => o.OfferingId == offeringId);
    }

    public Offering? GetOffering(int offeringId)
        => _offerings.FirstOrDefault(o => o.Id == offeringId && !o.IsDeleted);

    public bool OfferingExists(int offeringId)
        => _offerings.Any(o => o.Id == offeringId && !o.IsDeleted);

    public ServiceOffering AssignOffering(int professionalId, int offeringId)
    {
        if (!OfferingExists(offeringId)) throw new DomainException("Offering not found.");

        var assignment = new ServiceOffering(professionalId, offeringId);

        if (!_serviceOfferings.Add(assignment))
        {
            throw new DomainException("Offering is already assigned to this professional.");
        }

        return assignment;
    }

    public void UnassignOffering(int professionalId, int offeringId)
    {
        var assignment = new ServiceOffering(professionalId, offeringId);

        if (!_serviceOfferings.Remove(assignment))
        {
            throw new DomainException("Offering is not assigned to the professional.");
        }
    }

    public bool IsOfferingProvidedByProfessional(int professionalId, int offeringId)
    {
        return _serviceOfferings.Any(so => so.ProfessionalId == professionalId && so.OfferingId == offeringId);
    }
}