using Domain.Common;

namespace Domain.Entities;

public class Store : BaseEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Address { get; private set; } = null!;
    public string TaxIdNumber { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private readonly List<StoreManager> _managers = new();
    public IReadOnlyCollection<StoreManager> Managers => _managers.AsReadOnly();

    private readonly List<StoreProfessional> _staff = new();
    public IReadOnlyCollection<StoreProfessional> Staff => _staff.AsReadOnly();

    private readonly List<StoreOperatingHours> _operatingHours = new();
    public IReadOnlyCollection<StoreOperatingHours> OperatingHours => _operatingHours.AsReadOnly();

    private readonly List<StoreException> _exceptions = new();
    public IReadOnlyCollection<StoreException> Exceptions => _exceptions.AsReadOnly();

    private readonly List<Service> _services = new();
    internal ICollection<Service> Services => _services;

    private Store()
    {
        IsActive = true;
    }
}
