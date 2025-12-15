using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;

namespace Domain.Services;

public class StoreCatalogService
{
    private readonly IStoreCatalogRepository _storeCatalogRepository;
    private readonly IStaffRepository _staffRepository;

    public StoreCatalogService(IStoreCatalogRepository storeCatalogRepository, IStaffRepository staffRepository)
    {
        _storeCatalogRepository = storeCatalogRepository;
        _staffRepository = staffRepository;
    }

    public async Task<Service> AddService(int ownerId, int storeId, string name, decimal price, TimeSpan duration, string? description = null)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the offered services.");
        }

        var service = catalog.AddService(name, price, duration, description);
        await _storeCatalogRepository.SaveAsync(catalog);
        return service;
    }

    public async Task RemoveService(int ownerId, int storeId, int serviceId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the offered services.");
        }

        catalog.RemoveService(serviceId);
        await _storeCatalogRepository.SaveAsync(catalog);
    }

    public async Task<StaffService> AssignStaffToService(int ownerId, int storeId, int professionalId, int serviceId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the employee's offered services.");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("The professional is not working for the store.");
        }

        var staffService = catalog.AssignStaffToService(professionalId, serviceId);
        await _storeCatalogRepository.SaveAsync(catalog);
        return staffService;
    }

    public async Task UnassignStaffFromService(int ownerId, int storeId, int professionalId, int serviceId)
    {
        var catalog = await _storeCatalogRepository.GetByStoreAsync(storeId);
        var staff = await _staffRepository.GetByStoreAsync(storeId);

        if (!staff.IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can modify the employee's offered services.");
        }

        if (!staff.IsStaff(professionalId))
        {
            throw new DomainException("The professional is not working for the store.");
        }

        catalog.UnassignStaffFromService(professionalId, serviceId);
        await _storeCatalogRepository.SaveAsync(catalog);
    }
}