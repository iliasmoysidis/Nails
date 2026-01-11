using Domain.Enums;

namespace Domain.ValueObjects.Staff;

public sealed class StaffMemberRole
{
    public StaffRole Role { get; private set; }

    private StaffMemberRole() { }

    public StaffMemberRole(StaffRole role)
    {
        Role = role;
    }

    public override bool Equals(object? obj)
        => obj is StaffMemberRole other && Role == other.Role;

    public override int GetHashCode()
        => Role.GetHashCode();
}