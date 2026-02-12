using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public sealed class StaffMember
{
    public int Id { get; private set; }
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    public UtcDateTime JoinedAt { get; private set; }
    public UtcDateTime? LeftAt { get; private set; }

    public bool IsActive => LeftAt is null;

    private readonly List<StaffRoleAssignment> _roleHistory = new();
    public IReadOnlyCollection<StaffRoleAssignment> RoleHistory => _roleHistory.AsReadOnly();

    private StaffMember() { }

    private StaffMember(
        int storeId,
        int professionalId,
        IClock clock
        )
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
        JoinedAt = clock.Now;
        LeftAt = null;
    }

    public static StaffMember Join(int storeId, int professionalId, IEnumerable<StaffRole> roles, IClock clock)
    {
        var member = new StaffMember(storeId, professionalId, clock);

        foreach (var role in roles)
            member.AssignRole(role, clock);

        return member;
    }

    public void Leave(IClock clock)
    {
        EnsureActive();

        foreach (var assignment in _roleHistory.Where(r => r.IsActive))
            assignment.Remove(clock);

        LeftAt = clock.Now;
    }

    public bool HasRole(StaffRole role)
        => _roleHistory.Any(r => r.Role == role && r.IsActive);

    public bool HadRoleAt(StaffRole role, UtcDateTime time)
        => WasActiveAt(time) && _roleHistory.Any(r => r.Role == role && r.WasActiveAt(time));

    public bool WasActiveAt(UtcDateTime time)
        => JoinedAt <= time && (LeftAt is null || time < LeftAt);

    internal void AssignRole(StaffRole role, IClock clock)
    {
        EnsureActive();

        if (HasRole(role))
            throw new InvariantException($"Role {role} already assigned.");

        _roleHistory.Add(StaffRoleAssignment.Assign(role, clock));
    }

    internal void RemoveRole(StaffRole role, IClock clock)
    {
        EnsureActive();

        var active = _roleHistory.LastOrDefault(r => r.Role == role && r.IsActive);

        if (active is null)
            throw new NotFoundException($"Role {role} not assigned.");

        active.Remove(clock);
    }

    public bool HasAnyActiveRole()
        => _roleHistory.Any(r => r.IsActive);

    private void EnsureActive()
    {
        if (!IsActive)
            throw new StateException("Cannot modify an inactive staff member.");
    }
}