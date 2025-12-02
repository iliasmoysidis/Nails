using Domain.Exceptions;

namespace Domain.Entities;

public class StoreStaff
{
    public int StoreId { get; private set; }

    private readonly List<StoreOwner> _owners = new();
    public IReadOnlyCollection<StoreOwner> Owners => _owners.AsReadOnly();

    private readonly List<StoreEmployee> _staff = new();
    public IReadOnlyCollection<StoreEmployee> Staff => _staff.AsReadOnly();

    private StoreStaff() { }

    public static StoreStaff Create(int storeId)
    {
        return new StoreStaff
        {
            StoreId = storeId
        };
    }

    public bool IsOwner(int professionalId)
    {
        return _owners.Any(o => o.ProfessionalId == professionalId);
    }

    public bool IsStaff(int professionalId)
    {
        return _staff.Any(s => s.ProfessionalId == professionalId);
    }

    public StoreOwner AddOwner(int ownerId, int prospectiveOwnerId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add an owner.");
        }

        if (IsOwner(prospectiveOwnerId))
        {
            throw new DomainException("This professional is already an owner of this store.");
        }

        var newOwner = StoreOwner.Create(StoreId, prospectiveOwnerId);
        _owners.Add(newOwner);
        return newOwner;
    }

    public void RemoveOwner(int ownerId, int toBeRemovedOwnerId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove an owner.");
        }

        var toBeRemovedOwner = _owners.FirstOrDefault(s => s.ProfessionalId == toBeRemovedOwnerId);

        if (toBeRemovedOwner == null)
        {
            throw new DomainException("Professional is not an owner of this store.");
        }

        if (_owners.Count() == 1)
        {
            throw new DomainException("Cannot remove last owner.");
        }

        _owners.Remove(toBeRemovedOwner);
    }

    public StoreEmployee AddStaff(int ownerId, int professionalId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff.");
        }

        if (IsStaff(professionalId))
        {
            throw new DomainException("Professional already working for the store.");
        }

        var employee = StoreEmployee.Create(StoreId, professionalId);
        _staff.Add(employee);
        return employee;
    }

    public void RemoveStaff(int ownerId, int professionalId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove staff.");
        }

        var employee = _staff.FirstOrDefault(s => s.ProfessionalId == professionalId);

        if (employee == null)
        {
            throw new DomainException("Professional does not work for the store.");
        }

        _staff.Remove(employee);
    }
}