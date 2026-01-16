using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public sealed class StoreCatalogService
{
    private readonly IClock _clock;

    public StoreCatalogService(IClock clock)
    {
        _clock = clock;
    }

    public Offering AddOffering(StoreCatalog catalog, Staff staff, int ownerId, OfferingName name, Money price, Duration duration, string? description = null)
    {
        staff.EnsureOwner(ownerId);
        return catalog.AddOffering(name, price, duration, _clock, description);
    }

    public void RemoveOffering(StoreCatalog catalog, Staff staff, int ownerId, int offeringId)
    {
        staff.EnsureOwner(ownerId);
        catalog.RemoveOffering(offeringId, _clock);
    }

    public ServiceOffering AssignOffering(StoreCatalog catalog, Staff staff, int ownerId, int professionalId, int offeringId)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(professionalId);
        return catalog.AssignOffering(professionalId, offeringId);
    }

    public void UnassignOffering(StoreCatalog catalog, Staff staff, int ownerId, int professionalId, int offeringId)
    {
        staff.EnsureOwner(ownerId);
        staff.EnsureEmployee(professionalId);
        catalog.UnassignOffering(professionalId, offeringId);
    }
}