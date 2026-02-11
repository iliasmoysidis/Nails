using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly List<StaffMember> _members = new();
    public IReadOnlyCollection<StaffMember> Members => _members.AsReadOnly();

    private Staff() { }

    public static Staff Create(int storeId, int professionalId, IClock clock)
    {
        var staff = new Staff { StoreId = storeId };
        staff._members.Add(StaffMember.JoinAsOwner(storeId, professionalId, clock));
        return staff;
    }

    public void AddOwner(int professionalId, IClock clock)
        => AddRole(professionalId, StaffRole.Owner, clock);

    public void AddEmployee(int professionalId, IClock clock)
        => AddRole(professionalId, StaffRole.Employee, clock);

    public void RemoveOwner(int professionalId, IClock clock)
    {
        var member = GetActiveMemberOrThrow(professionalId);
        EnsureNotLastActiveOwner(member);
        RemoveRole(member, StaffRole.Owner, clock);
    }

    public void RemoveEmployee(int professionalId, IClock clock)
    {
        var member = GetActiveMemberOrThrow(professionalId);
        RemoveRole(member, StaffRole.Employee, clock);
    }

    public void Clear(IClock clock)
    {
        foreach (var member in _members.Where(m => m.IsActive))
            member.Leave(clock);
    }

    public bool IsOwner(int professionalId)
        => GetActiveMember(professionalId)?.HasRole(StaffRole.Owner) == true;

    public bool IsEmployee(int professionalId)
        => GetActiveMember(professionalId)?.HasRole(StaffRole.Employee) == true;

    public bool IsStaff(int professionalId)
        => _members.Any(m => m.IsActive && m.ProfessionalId == professionalId);

    public void EnsureOwner(int professionalId)
    {
        if (!IsOwner(professionalId))
            throw new ForbiddenException("Only an owner can perform this action.");
    }

    public void EnsureEmployee(int professionalId)
    {
        if (!IsEmployee(professionalId))
            throw new ForbiddenException("Professional does not work for the store.");
    }

    private void EnsureNotLastActiveOwner(StaffMember member)
    {
        if (!member.HasRole(StaffRole.Owner))
            return;

        var ownerCount = _members.Count(m => m.IsActive && m.HasRole(StaffRole.Owner));
        if (ownerCount == 1)
            throw new InvariantException("Cannot remove last owner.");
    }

    private void EnsureNotAlreadyActive(int professionalId)
    {
        if (_members.Any(m => m.IsActive && m.ProfessionalId == professionalId))
            throw new InvariantException("Professional is already active staff.");
    }

    private void AddRole(int professionalId, StaffRole role, IClock clock)
    {
        var member = GetActiveMember(professionalId);

        if (member is null)
        {
            EnsureNotAlreadyActive(professionalId);
            _members.Add(CreateMemberWithRole(professionalId, role, clock));
        }

        else
            member.AddRole(role);
    }

    private void RemoveRole(StaffMember member, StaffRole role, IClock clock)
    {
        member.RemoveRole(role);
        RemoveIfRoleless(member, clock);
    }

    private void RemoveIfRoleless(StaffMember member, IClock clock)
    {
        if (member.Roles.Count == 0)
            member.Leave(clock);
    }

    private StaffMember? GetActiveMember(int professionalId)
        => _members.FirstOrDefault(m => m.ProfessionalId == professionalId && m.IsActive);

    private StaffMember GetActiveMemberOrThrow(int professionalId)
        => GetActiveMember(professionalId)
            ?? throw new NotFoundException("Professional is not active staff.");

    private StaffMember CreateMemberWithRole(int professionalId, StaffRole role, IClock clock)
        => role switch
        {
            StaffRole.Owner => StaffMember.JoinAsOwner(StoreId, professionalId, clock),
            StaffRole.Employee => StaffMember.JoinAsEmployee(StoreId, professionalId, clock),
            _ => throw new StateException("Invalid staff role.")
        };
}