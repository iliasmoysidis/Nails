using System.Threading.Tasks;
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

}