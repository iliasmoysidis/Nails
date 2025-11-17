using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Exceptions;

namespace Domain.Entities;

public class Store : HistoricEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;

    private readonly List<StoreOwner> _owners = new();
    public IReadOnlyCollection<StoreOwner> Owners => _owners.AsReadOnly();

    private readonly List<StoreProfessional> _staff = new();
    public IReadOnlyCollection<StoreProfessional> Staff => _staff.AsReadOnly();

    private readonly List<Service> _services = new();
    private IReadOnlyCollection<Service> Services => _services;

    private readonly List<ProfessionalService> _professionalServices = new();
    private IReadOnlyCollection<ProfessionalService> ProfessionalServices => _professionalServices;

    private readonly List<StoreOperatingHours> _operatingHours = new();
    public IReadOnlyCollection<StoreOperatingHours> OperatingHours => _operatingHours.AsReadOnly();

    private readonly List<StoreException> _exceptions = new();
    public IReadOnlyCollection<StoreException> Exceptions => _exceptions.AsReadOnly();

    private Store() { }

    public bool IsOwner(int professionalId)
    {
        return _owners.Any(o => o.ProfessionalId == professionalId);
    }

    public bool IsStaff(int professionalId)
    {
        return _staff.Any(s => s.ProfessionalId == professionalId);
    }

    public bool IsProvided(int profesionalId, int serviceId)
    {
        return _professionalServices.Any(ps => ps.ProfessionalId == profesionalId && ps.ServiceId == serviceId);
    }

    public static Store Create(string name, string address, string taxIdNumber, string email, string phone)
    {
        ValidateStoreInfo(name, address, taxIdNumber, email, phone);
        return new Store
        {
            Name = name,
            Address = address,
            TaxIdNumber = taxIdNumber,
            Email = email,
            Phone = phone
        };
    }

    public StoreOwner AddOwner(int ownerId, int professionalId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add owner to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add an owner.");
        }

        if (IsOwner(professionalId))
        {
            throw new DomainException("This professional is already an owner of this store.");
        }

        var owner = StoreOwner.Create(Id, professionalId);
        _owners.Add(owner);
        MarkAsUpdated();

        return owner;
    }

    public void RemoveOwner(int ownerId, int oldOwnerId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove owner from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove an owner.");
        }

        var oldOwner = _owners.FirstOrDefault(s => s.ProfessionalId == oldOwnerId);

        if (oldOwner == null)
        {
            throw new DomainException("Professional is not an owner of this store.");
        }

        if (_owners.Count() == 1)
        {
            throw new DomainException("Cannot remove last owner.");
        }

        _owners.Remove(oldOwner);
        MarkAsUpdated();
    }

    public StoreProfessional AddStaff(int ownerId, int professionalId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add staff to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff.");
        }

        if (IsStaff(professionalId))
        {
            throw new DomainException("Professional already working for the store.");
        }

        var employee = StoreProfessional.Create(Id, professionalId);
        _staff.Add(employee);
        MarkAsUpdated();

        return employee;
    }

    public void RemoveStaff(int ownerId, int professionalId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove staff from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove staff.");
        }

        var employee = _staff.FirstOrDefault(s => s.ProfessionalId == professionalId);

        if (employee == null)
        {
            throw new DomainException("Professional does not work for the store.");
        }

        _staff.Remove(employee);
        MarkAsUpdated();
    }

    public Service AddService(int ownerId, string name, decimal price, TimeSpan duration)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot add service to an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add a service.");
        }

        var service = Service.Create(this, name, price, duration);
        _services.Add(service);
        MarkAsUpdated();

        return service;
    }

    public void RemoveService(int ownerId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot remove service from an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove a service.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Cannot remove a service that is not offered by the store.");
        }

        service.Deactivate();
        MarkAsUpdated();
    }

    public ProfessionalService AssignStaffToService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot assign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can assign a staff to a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot assign services to a professional who does not work at the store.");
        }

        if (IsProvided(professionalId, serviceId))
        {
            throw new DomainException("Service is already offered by this professional");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var professionalService = ProfessionalService.Create(professionalId, serviceId);
        _professionalServices.Add(professionalService);
        MarkAsUpdated();

        return professionalService;
    }

    public void UnassignStaffFromService(int ownerId, int professionalId, int serviceId)
    {
        if (IsDeleted)
        {
            throw new DomainException("Cannot unassign services in an inactive store.");
        }

        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can unassign a staff from a service.");
        }

        if (!IsStaff(professionalId))
        {
            throw new DomainException("Cannot unassign services from professionals who do not work at the store.");
        }

        var service = _services.FirstOrDefault(s => s.Id == serviceId && s.StoreId == Id);

        if (service == null)
        {
            throw new DomainException("Service is not offered by the store.");
        }

        var professionalService = _professionalServices.FirstOrDefault(ps => ps.ProfessionalId == professionalId && ps.ServiceId == serviceId);

        if (professionalService == null)
        {
            throw new DomainException("Service is not offered by the professional.");
        }

        _professionalServices.Remove(professionalService);
        MarkAsUpdated();
    }

    private static void ValidateStoreInfo(string name, string address, string taxIdNumber, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name is required.");
        }

        if (name.Length > 100)
        {
            throw new DomainException("Name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email is required.");
        }

        if (!IsValidEmail(email))
        {
            throw new DomainException("Invalid email format.");
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new DomainException("Phone is required.");
        }

        if (phone.Length > 20)
        {
            throw new DomainException("Phone cannot exceed 20 characters.");
        }

        if (string.IsNullOrWhiteSpace(taxIdNumber))
        {
            throw new DomainException("Tax ID number is required.");
        }

        if (taxIdNumber.Length > 50)
        {
            throw new DomainException("Tax ID number cannot exceed 50 characters.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
