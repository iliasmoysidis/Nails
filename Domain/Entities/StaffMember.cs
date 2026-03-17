using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class StaffMember
{
    public int ProfessionalId { get; }

    private readonly HashSet<StaffRole> _roles = new();
    public IReadOnlyCollection<StaffRole> Roles => _roles;

    private StaffMember() { }

    private StaffMember(int professionalId, IEnumerable<StaffRole> roles)
    {
        ProfessionalId = professionalId;

        foreach (var role in roles)
            _roles.Add(role);
    }

    public static StaffMember Create(int professionalId, IEnumerable<StaffRole> roles)
        => new(professionalId, roles);

    public bool HasRole(StaffRole role)
        => _roles.Contains(role);

    public void AddRole(StaffRole role)
    {
        if (!_roles.Add(role))
            throw new InvariantException($"Role {role} already assigned.");
    }

    public void RemoveRole(StaffRole role)
    {
        if (!_roles.Remove(role))
            throw new NotFoundException($"Role {role} not assigned.");
    }

    public bool HasAnyRole()
        => _roles.Count > 0;
}