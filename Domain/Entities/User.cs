using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Phone { get; private set; } = null!;
    public string? TaxIdNumber { get; private set; }
    public bool IsCustomer { get; private set; } = true;
    public bool IsProfessional { get; private set; } = false;
    public bool IsActive { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    private readonly List<StoreProfessional> _workplaces = new();
    public IReadOnlyCollection<StoreProfessional> Workplaces => _workplaces.AsReadOnly();

    private readonly List<ProfessionalService> _providedServices = new();
    public IReadOnlyCollection<ProfessionalService> ProvidedServices => _providedServices.AsReadOnly();

    private readonly List<ProfessionalSchedule> _workSchedules = new();
    public IReadOnlyCollection<ProfessionalSchedule> WorkSchedules => _workSchedules.AsReadOnly();

    private readonly List<ProfessionalTimeOff> _timeOffs = new();
    public IReadOnlyCollection<ProfessionalTimeOff> TimeOffs => _timeOffs.AsReadOnly();

    private readonly List<StoreManager> _managedStores = new();
    public IReadOnlyCollection<StoreManager> ManagedStores => _managedStores.AsReadOnly();

    private User()
    {
        IsCustomer = true;
        IsProfessional = false;
        IsActive = true;
    }
}
