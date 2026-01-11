using Domain.Enums;
using Domain.ValueObjects.Staff;

namespace Domain.Entities;

public sealed class StaffMember
{
    public int StoreId { get; }
    public int ProfessionalId { get; }

    private readonly HashSet<StaffMemberRole> _roles;
    public IReadOnlyCollection<StaffMemberRole> Roles => _roles.AsReadOnly();

    private StaffMember(int storeId, int professionalId, IEnumerable<StaffMemberRole> roles)
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
        _roles = new HashSet<StaffMemberRole>(roles);
    }

    public static StaffMember CreateOwner(int storeId, int professionalId)
        => new(storeId, professionalId, [new StaffMemberRole(StaffRole.Owner)]);


    public static StaffMember CreateEmployee(int storeId, int professionalId)
        => new(storeId, professionalId, [new StaffMemberRole(StaffRole.Employee)]);

    public bool HasRole(StaffRole role) => _roles.Any(r => r.Role == role);

    public void AddRole(StaffRole role) => _roles.Add(new StaffMemberRole(role));

    public void RemoveRole(StaffRole role)
    {
        var existing = _roles.FirstOrDefault(r => r.Role == role);
        if (existing is not null)
            _roles.Remove(existing);
    }
}