using Domain.Exceptions;

namespace Domain.Entities;

public class StoreServiceManager
{
    public int StoreId { get; private set; }

    private readonly List<Service> _services = new();
    public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

    private readonly List<StaffService> _staffServices = new();
    public IReadOnlyCollection<StaffService> StaffServices => _staffServices.AsReadOnly();

    private StoreServiceManager() { }

    public static StoreServiceManager Create(int storeId)
    {
        return new StoreServiceManager
        {
            StoreId = storeId
        };
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
        _staffServices.RemoveAll(s => s.ServiceId == serviceId);
    }

    public StaffService AssignStaffToService(int professionalId, int serviceId)
    {
        if (ServiceIsProvidedByProfessional(professionalId, serviceId))
        {
            throw new DomainException("Service is already offered by this professional.");
        }

        if (!ServiceIsProvidedByTheStore(serviceId))
        {
            throw new DomainException("Cannot assign staff to a non-existing or inactive service.");
        }

        var staffService = StaffService.Create(professionalId, serviceId);
        _staffServices.Add(staffService);
        return staffService;
    }

    public void UnassignStaffFromService(int professionalId, int serviceId)
    {
        if (!ServiceIsProvidedByTheStore(serviceId))
        {
            throw new DomainException("Cannot unassign staff from a non-existing or inactive service.");
        }

        var staffService = _staffServices.FirstOrDefault(
            ps => ps.ProfessionalId == professionalId &&
            ps.ServiceId == serviceId);

        if (staffService == null)
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        _staffServices.Remove(staffService);
    }

    public bool ServiceIsProvidedByProfessional(int professionalId, int serviceId)
    {
        return _staffServices.Any(ps => ps.ProfessionalId == professionalId && ps.ServiceId == serviceId);
    }

    public bool ServiceIsProvidedByTheStore(int serviceId)
    {
        return _services.Any(s => s.Id == serviceId && !s.IsDeleted);
    }
}