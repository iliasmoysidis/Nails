using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly HashSet<StaffMember> _members = new();
    public IReadOnlyCollection<StaffMember> Members => _members;

    private Staff() { }

    public static Staff Create(int storeId, int professionalId)
    {
        var staff = new Staff { StoreId = storeId };
        staff._members.Add(StaffMember.CreateOwner(storeId, professionalId));
        return staff;
    }

    public void AddOwner(int professionalId)
        => AddRole(professionalId, StaffRole.Owner);

    public void AddEmployee(int professionalId)
        => AddRole(professionalId, StaffRole.Employee);

    public void RemoveOwner(int professionalId)
    {
        var member = GetMemberOrThrow(professionalId);
        EnsureNotLastOwner(member);
        RemoveRole(member, StaffRole.Owner);
    }

    public void RemoveEmployee(int professionalId)
    {
        var member = GetMemberOrThrow(professionalId);
        RemoveRole(member, StaffRole.Employee);
    }

    public bool IsOwner(int professionalId)
        => GetMember(professionalId)?.HasRole(StaffRole.Owner) == true;

    public bool IsEmployee(int professionalId)
        => GetMember(professionalId)?.HasRole(StaffRole.Employee) == true;

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

    private void EnsureNotLastOwner(StaffMember member)
    {
        if (!member.HasRole(StaffRole.Owner))
            return;

        var ownerCount = _members.Count(m => m.HasRole(StaffRole.Owner));
        if (ownerCount == 1)
            throw new InvariantException("Cannot remove last owner.");
    }

    private void AddRole(int professionalId, StaffRole role)
    {
        var member = GetMember(professionalId);

        if (member is null)
            _members.Add(CreateMemberWithRole(professionalId, role));
        else
            member.AddRole(role);
    }

    private void RemoveRole(StaffMember member, StaffRole role)
    {
        member.RemoveRole(role);
        RemoveIfRoleless(member);
    }

    private void RemoveIfRoleless(StaffMember member)
    {
        if (member.Roles.Count == 0) _members.Remove(member);
    }

    private StaffMember? GetMember(int professionalId)
        => _members.FirstOrDefault(m => m.ProfessionalId == professionalId);

    private StaffMember GetMemberOrThrow(int professionalId)
        => GetMember(professionalId)
            ?? throw new NotFoundException("Professional is not part of this store.");

    private StaffMember CreateMemberWithRole(int professionalId, StaffRole role)
        => role switch
        {
            StaffRole.Owner => StaffMember.CreateOwner(StoreId, professionalId),
            StaffRole.Employee => StaffMember.CreateEmployee(StoreId, professionalId),
            _ => throw new StateException("Invalid staff role.")
        };
}