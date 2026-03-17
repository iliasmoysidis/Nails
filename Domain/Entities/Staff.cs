using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; }

    private readonly List<StaffMember> _members = new();
    public IReadOnlyCollection<StaffMember> Members => _members;

    private Staff() { }

    private Staff(int storeId)
    {
        StoreId = storeId;
    }

    public static Staff Create(int storeId, int ownerProfessionalId)
    {
        var staff = new Staff(storeId);

        staff._members.Add(
            StaffMember.Create(ownerProfessionalId, [StaffRole.Owner])
        );

        return staff;
    }

    public bool IsStaff(int professionalId)
        => GetMember(professionalId) is not null;

    public bool IsOwner(int professionalId)
        => GetMember(professionalId)?.HasRole(StaffRole.Owner) == true;

    public bool IsEmployee(int professionalId)
        => GetMember(professionalId)?.HasRole(StaffRole.Employee) == true;

    public void AddOwner(int professionalId)
        => AddOrAssignRole(professionalId, StaffRole.Owner);

    public void AddEmployee(int professionalId)
        => AddOrAssignRole(professionalId, StaffRole.Employee);

    public void RemoveOwner(int professionalId)
    {
        var member = GetMemberOrThrow(professionalId);

        if (!member.HasRole(StaffRole.Owner))
            return;

        EnsureNotLastOwner(member);

        member.RemoveRole(StaffRole.Owner);

        RemoveIfRoleless(member);
    }

    public void RemoveEmployee(int professionalId)
    {
        var member = GetMemberOrThrow(professionalId);

        if (!member.HasRole(StaffRole.Employee))
            return;

        member.RemoveRole(StaffRole.Employee);

        RemoveIfRoleless(member);
    }

    public void RemoveFromStaff(int professionalId)
    {
        var member = GetMemberOrThrow(professionalId);

        if (member.HasRole(StaffRole.Owner))
            EnsureNotLastOwner(member);

        _members.Remove(member);
    }

    public void Clear()
        => _members.Clear();

    private void AddOrAssignRole(int professionalId, StaffRole role)
    {
        var member = GetMember(professionalId);

        if (member is null)
        {
            _members.Add(
                StaffMember.Create(professionalId, [role])
            );

            return;
        }

        member.AddRole(role);
    }

    private void RemoveIfRoleless(StaffMember member)
    {
        if (!member.HasAnyRole())
            _members.Remove(member);
    }

    private void EnsureNotLastOwner(StaffMember member)
    {
        var owners = _members.Count(m => m.HasRole(StaffRole.Owner));

        if (member.HasRole(StaffRole.Owner) && owners == 1)
            throw new InvariantException("Cannot remove last owner.");
    }

    private StaffMember? GetMember(int professionalId)
        => _members.FirstOrDefault(m => m.ProfessionalId == professionalId);

    private StaffMember GetMemberOrThrow(int professionalId)
        => GetMember(professionalId)
           ?? throw new NotFoundException("Professional is not active staff.");
}