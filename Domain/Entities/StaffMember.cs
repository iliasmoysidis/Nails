using Domain.Enums;

namespace Domain.Entities;

public sealed record StaffMember
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private HashSet<StaffRole> _roles = new();
    public IReadOnlyCollection<StaffRole> Roles => _roles;

    private StaffMember() { }

    private StaffMember(
        int storeId,
        int professionalId,
        IEnumerable<StaffRole> roles
        )
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
        _roles = new HashSet<StaffRole>(roles);
    }

    public static StaffMember CreateOwner(int storeId, int professionalId)
        => new(storeId, professionalId, [StaffRole.Owner]);


    public static StaffMember CreateEmployee(int storeId, int professionalId)
        => new(storeId, professionalId, [StaffRole.Employee]);

    public bool HasRole(StaffRole role) => _roles.Contains(role);
    public void AddRole(StaffRole role) => _roles.Add(role);
    public void RemoveRole(StaffRole role) => _roles.Remove(role);

    public bool Equals(StaffMember? other)
        => other is not null
            && StoreId == other.StoreId
            && ProfessionalId == other.ProfessionalId;

    public override int GetHashCode()
        => HashCode.Combine(StoreId, ProfessionalId);
}