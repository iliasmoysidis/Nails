using Domain.Common.Exceptions;
using Domain.Roster.EnumObjects;
using Domain.Roster.ValueObjects;

namespace Domain.Roster.Entities;

public class StaffMember
{
    public int ProfessionalId { get; }

    private readonly List<RoleAssignment> _roles = new();
    public IReadOnlyCollection<RoleAssignment> Roles => _roles.AsReadOnly();

    private StaffMember() { }

    private StaffMember(int professionalId, IEnumerable<StaffRole> roles)
    {
        ProfessionalId = professionalId;

        foreach (var role in roles.Distinct())
            _roles.Add(new RoleAssignment(role));
    }

    public static StaffMember Create(int professionalId, IEnumerable<StaffRole> roles)
        => new(professionalId, roles);

    public bool HasRole(StaffRole role)
        => _roles.Any(r => r.Role == role);

    public void AddRole(StaffRole role)
    {
        if (HasRole(role))
            throw new InvariantException($"Role {role} already assigned.");

        _roles.Add(new RoleAssignment(role));
    }

    public void RemoveRole(StaffRole role)
    {
        var assignment = _roles.FirstOrDefault(r => r.Role == role);

        if (assignment is null)
            throw new NotFoundException($"Role {role} not assigned.");

        _roles.Remove(assignment);
    }

    public bool HasAnyRole()
        => _roles.Count > 0;
}
