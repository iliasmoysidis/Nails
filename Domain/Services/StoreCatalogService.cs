using Domain.Entities;
using Domain.Interfaces;
using Domain.Repositories;
using Domain.ValueObjects.Finance;
using Domain.ValueObjects.Offerings;
using Domain.ValueObjects.Time;

namespace Domain.Services;

public class StoreCatalogService
{
    private readonly IStoreCatalogRepository _storeCatalogRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IClock _clock;

    public StoreCatalogService(IStoreCatalogRepository storeCatalogRepository, IStaffRepository staffRepository, IClock clock)
    {
        _storeCatalogRepository = storeCatalogRepository;
        _staffRepository = staffRepository;
        _clock = clock;
    }

    public async Task<Offering> AddOffering(int ownerId, int storeId, OfferingName name, Money price, Duration duration, string? description = null)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        staff.EnsureOwner(ownerId);

        var offering = catalog.AddOffering(name, price, duration, _clock, description);

        await _storeCatalogRepository.SaveAsync(catalog);

        return offering;
    }

    public async Task RemoveOffering(int ownerId, int storeId, int offeringId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        staff.EnsureOwner(ownerId);

        catalog.RemoveOffering(offeringId, _clock);

        await _storeCatalogRepository.SaveAsync(catalog);
    }

    public async Task<ServiceOffering> AssignOffering(int ownerId, int storeId, int professionalId, int offeringId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        staff.EnsureOwner(ownerId);
        staff.EnsureStaff(professionalId);

        var assignment = catalog.AssignOffering(professionalId, offeringId);

        await _storeCatalogRepository.SaveAsync(catalog);

        return assignment;
    }

    public async Task UnassignOffering(int ownerId, int storeId, int professionalId, int offeringId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        staff.EnsureOwner(ownerId);
        staff.EnsureStaff(professionalId);

        catalog.UnassignOffering(professionalId, offeringId);

        await _storeCatalogRepository.SaveAsync(catalog);
    }
}