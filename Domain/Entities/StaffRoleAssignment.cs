using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public sealed class StaffRoleAssignment
{
    public StaffRole Role { get; private set; }
    public UtcDateTime AssignedAt { get; private set; }
    public UtcDateTime? RemovedAt { get; private set; }

    public bool IsActive => RemovedAt is null;

    private StaffRoleAssignment() { }

    private StaffRoleAssignment(StaffRole role, UtcDateTime assignedAt)
    {
        Role = role;
        AssignedAt = assignedAt;
        RemovedAt = null;
    }

    public static StaffRoleAssignment Assign(StaffRole role, IClock clock)
        => new(role, clock.Now);

    public void Remove(IClock clock)
    {
        if (!IsActive)
            throw new StateException("Role assignment already removed.");

        RemovedAt = clock.Now;
    }

    public bool WasActiveAt(UtcDateTime time)
        => AssignedAt <= time && (RemovedAt is null || time < RemovedAt);
}