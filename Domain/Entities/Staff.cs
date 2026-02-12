using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly List<StaffMember> _members = new();
    public IReadOnlyCollection<StaffMember> Members => _members.AsReadOnly();

    private Staff() { }

    private Staff(int storeId)
    {
        StoreId = storeId;
    }

    public static Staff Create(int storeId, int professionalId, IClock clock)
    {
        var staff = new Staff(storeId);

        staff._members.Add(StaffMember.Join(
            storeId,
            professionalId,
            [StaffRole.Owner],
            clock)
        );

        return staff;
    }

    public bool IsStaff(int professionalId)
        => GetActiveMember(professionalId) is not null;

    public bool IsOwner(int professionalId)
        => GetActiveMember(professionalId)?.HasRole(StaffRole.Owner) == true;

    public bool IsEmployee(int professionalId)
        => GetActiveMember(professionalId)?.HasRole(StaffRole.Employee) == true;

    public IReadOnlyCollection<int> ActiveProfessionalIdsAt(UtcDateTime time)
        => _members
            .Where(m => m.WasActiveAt(time))
            .Select(m => m.ProfessionalId)
            .ToArray();

    public bool WasOwnerAt(int professionalId, UtcDateTime time)
        => _members.Any(m => m.ProfessionalId == professionalId && m.HadRoleAt(StaffRole.Owner, time));

    public bool WasEmployeeAt(int professionalId, UtcDateTime time)
        => _members.Any(m => m.ProfessionalId == professionalId && m.HadRoleAt(StaffRole.Employee, time));

    public IReadOnlyCollection<int> OwnerProfessionalIdsAt(UtcDateTime time)
        => _members
            .Where(m => m.WasActiveAt(time) && m.HadRoleAt(StaffRole.Owner, time))
            .Select(m => m.ProfessionalId)
            .ToArray();

    public void AddOwner(int professionalId, IClock clock)
        => AddOrAssignRole(professionalId, StaffRole.Owner, clock);

    public void AddEmployee(int professionalId, IClock clock)
        => AddOrAssignRole(professionalId, StaffRole.Employee, clock);

    public void RemoveOwner(int professionalId, IClock clock)
    {
        var member = GetActiveMemberOrThrow(professionalId);

        if (!member.HasRole(StaffRole.Owner)) return;

        EnsureNotLastActiveOwnerRemoving(member);

        member.RemoveRole(StaffRole.Owner, clock);

        RemoveIfRoleless(member, clock);
    }

    public void RemoveEmployee(int professionalId, IClock clock)
    {
        var member = GetActiveMemberOrThrow(professionalId);

        if (!member.HasRole(StaffRole.Employee)) return;

        member.RemoveRole(StaffRole.Employee, clock);

        RemoveIfRoleless(member, clock);
    }

    public void RemoveFromStaff(int professionalId, IClock clock)
    {
        var member = GetActiveMemberOrThrow(professionalId);

        if (member.HasRole(StaffRole.Owner))
            EnsureNotLastActiveOwnerRemoving(member);

        member.Leave(clock);
    }

    public void Clear(IClock clock)
    {
        foreach (var member in _members.Where(m => m.IsActive))
            member.Leave(clock);
    }

    public void EnsureOwner(int professionalId)
    {
        if (!IsOwner(professionalId))
            throw new ForbiddenException("Only an owner can perform this action.");
    }

    public void EnsureEmployee(int professionalId)
    {
        if (!IsEmployee(professionalId))
            throw new ForbiddenException("Only an employee can perform this action.");
    }

    private void AddOrAssignRole(int professionalId, StaffRole role, IClock clock)
    {
        var member = GetActiveMember(professionalId);

        if (member is null)
        {
            _members.Add(StaffMember.Join(StoreId, professionalId, [role], clock));

            return;
        }

        member.AssignRole(role, clock);
    }

    private void RemoveIfRoleless(StaffMember member, IClock clock)
    {
        if (!member.HasAnyActiveRole())
            member.Leave(clock);
    }

    private void EnsureNotLastActiveOwnerRemoving(StaffMember member)
    {
        var owners = _members.Count(m => m.IsActive && m.HasRole(StaffRole.Owner));

        if (member.HasRole(StaffRole.Owner) && owners == 1)
            throw new InvariantException("Cannot remove last owner.");
    }

    private StaffMember? GetActiveMember(int professionalId)
        => _members.FirstOrDefault(m => m.IsActive && m.ProfessionalId == professionalId);

    private StaffMember GetActiveMemberOrThrow(int professionalId)
        => GetActiveMember(professionalId)
            ?? throw new NotFoundException("Professional is not active staff.");
}