using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public sealed class StaffMember
{
    public int Id { get; private set; }
    public int StoreId { get; }
    public int ProfessionalId { get; }

    public UtcDateTime JoinedAt { get; }
    public UtcDateTime? LeftAt { get; private set; }

    public bool IsActive => LeftAt is null;

    private HashSet<StaffRole> _roles = new();
    public IReadOnlyCollection<StaffRole> Roles => _roles;

    private StaffMember() { }

    private StaffMember(
        int storeId,
        int professionalId,
        UtcDateTime joinedAt,
        IEnumerable<StaffRole> roles
        )
    {
        StoreId = storeId;
        ProfessionalId = professionalId;
        JoinedAt = joinedAt;
        LeftAt = null;
        _roles = new HashSet<StaffRole>(roles);
    }

    public static StaffMember JoinAsOwner(int storeId, int professionalId, IClock clock)
        => new(storeId, professionalId, clock.Now, [StaffRole.Owner]);


    public static StaffMember JoinAsEmployee(int storeId, int professionalId, IClock clock)
        => new(storeId, professionalId, clock.Now, [StaffRole.Employee]);

    public void Leave(IClock clock)
    {
        if (!IsActive)
            throw new StateException("Staff member already left.");

        LeftAt = clock.Now;
    }

    public bool HasRole(StaffRole role) => _roles.Contains(role);
    public void AddRole(StaffRole role)
    {
        EnsureActive();
        _roles.Add(role);
    }
    public void RemoveRole(StaffRole role)
    {
        EnsureActive();
        _roles.Remove(role);
    }

    public bool WasActiveAt(UtcDateTime time)
        => JoinedAt <= time && (LeftAt is null || time < LeftAt);

    private void EnsureActive()
    {
        if (!IsActive)
            throw new StateException("Cannot modify roles of inactive staff member.");
    }
}