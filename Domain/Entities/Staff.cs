using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly HashSet<StaffMember> _members = new();
    public IReadOnlyCollection<StaffMember> Members => _members.AsReadOnly();

    private Staff() { }

    public static Staff Create(int storeId, int initialOwnerId)
    {
        return new Staff
        {
            StoreId = storeId,
            _members =
            {
                StaffMember.CreateOwner(storeId, initialOwnerId)
            }
        };
    }

    public void AddOwner(int professionalId)
    {
        var member = _members.FirstOrDefault(m => m.ProfessionalId == professionalId);

        if (member is null)
        {
            _members.Add(StaffMember.CreateOwner(StoreId, professionalId));
        }
        else
        {
            member.AddRole(StaffRole.Owner);
        }
    }

    public void RemoveOwner(int professionalId)
    {
        var member = _members.FirstOrDefault(m => m.ProfessionalId == professionalId)
            ?? throw new DomainException("Professional is not part of this store.");

        if (member.HasRole(StaffRole.Owner) && _members.Count(m => m.HasRole(StaffRole.Owner)) == 1)
            throw new DomainException("Cannot remove last owner.");

        member.RemoveRole(StaffRole.Owner);

        if (!member.Roles.Any())
            _members.Remove(member);
    }

    public void AddEmployee(int professionalId)
    {
        var member = _members.FirstOrDefault(m => m.ProfessionalId == professionalId);

        if (member is null)
            _members.Add(StaffMember.CreateEmployee(StoreId, professionalId));

        else
            member.AddRole(StaffRole.Employee);
    }

    public void RemoveEmployee(int professionalId)
    {
        var member = _members.FirstOrDefault(m => m.ProfessionalId == professionalId)
            ?? throw new DomainException("Professional is not part of this store.");

        member.RemoveRole(StaffRole.Employee);

        if (!member.Roles.Any())
            _members.Remove(member);
    }

    public bool IsOwner(int professionalId)
        => _members.Any(m =>
                m.ProfessionalId == professionalId &&
                m.HasRole(StaffRole.Owner));

    public bool IsEmployee(int professionalId)
        => _members.Any(m =>
                m.ProfessionalId == professionalId &&
                m.HasRole(StaffRole.Employee));

    public void EnsureOwner(int professionalId)
    {
        if (!IsOwner(professionalId))
            throw new DomainException("Only an owner can perform this action.");
    }

    public void EnsureEmployee(int professionalId)
    {
        if (!IsEmployee(professionalId))
            throw new DomainException("Professional does not work for the store.");
    }
}