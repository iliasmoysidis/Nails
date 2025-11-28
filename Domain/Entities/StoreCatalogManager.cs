using Domain.Exceptions;

namespace Domain.Entities;

public class StoreCatalogManager
{
    public int StoreId { get; private set; }

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    private readonly List<ProfessionalService> _staffServices = new();
    public IReadOnlyCollection<ProfessionalService> StaffServices => _staffServices.AsReadOnly();

    private StoreCatalogManager() { }

    public StoreCatalogManager Create(int storeId)
    {
        return new StoreCatalogManager
        {
            StoreId = storeId
        };
    }

    public bool ServiceIsProvidedByProfessional(int professionalId, int serviceId)
    {
        return _staffServices.Any(ps => ps.ProfessionalId == professionalId && ps.ServiceId == serviceId);
    }

    public bool ServiceIsProvidedByTheStore(int serviceId)
    {
        return _services.Any(s => s.Id == serviceId && !s.IsDeleted);
    }

    public Service AddService(string name, decimal price, TimeSpan duration, string? description = null)
    {
        var service = Service.Create(StoreId, name, price, duration, description);
        _services.Add(service);
        return service;
    }

    public void RemoveService(int serviceId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        service.Deactivate();
        service.MarkAsUpdated();
    }

    public ProfessionalService AssignStaffToService(int ownerId, int professionalId, int serviceId)
    {
        if (ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("Service is already offered by this professional");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        var professionalService = ProfessionalService.Create(professionalId, serviceId);
        _staffServices.Add(professionalService);
        return professionalService;
    }

    public void UnassignStaffFromService(int ownerId, int professionalId, int serviceId)
    {
        var service = _services.FirstOrDefault(s => s.Id == serviceId && !s.IsDeleted);

        if (service == null)
        {
            throw new DomainException("Could not find service.");
        }

        var professionalService = _staffServices.FirstOrDefault(
            ps => ps.ProfessionalId == professionalId &&
            ps.ServiceId == serviceId);

        if (professionalService == null)
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        _staffServices.Remove(professionalService);
    }
}