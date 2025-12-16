using Domain.Exceptions;
using Domain.ValueObjects.Store;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly HashSet<Owner> _owners = new();
    public IReadOnlyCollection<Owner> Owners => _owners.AsReadOnly();

    private readonly HashSet<Employee> _employees = new();
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Staff() { }

    public static Staff Create(int storeId, int initialOwnerId)
    {
        return new Staff
        {
            StoreId = storeId,
            _owners = { new Owner(storeId, initialOwnerId) }
        };
    }

    public bool IsOwner(int professionalId)
    {
        return _owners.Any(o => o.ProfessionalId == professionalId);
    }

    public bool IsStaff(int professionalId)
    {
        return _employees.Any(s => s.ProfessionalId == professionalId);
    }

    public void AddOwner(int actingOwnerId, int newOwnerId)
    {
        EnsureOwner(actingOwnerId);
        if (!_owners.Add(new Owner(StoreId, newOwnerId)))
            throw new DomainException("Professional is already an owner of this store.");
    }

    public void RemoveOwner(int actingOwnerId, int ownerToRemoveId)
    {
        EnsureOwner(actingOwnerId);

        if (_owners.Count == 1)
            throw new DomainException("Cannot remove last owner.");

        if (!_owners.Remove(new Owner(StoreId, ownerToRemoveId)))
            throw new DomainException("Professional is not an owner of this store.");
    }

    public void AddStaff(int actingOwnerId, int professionalId)
    {
        EnsureOwner(actingOwnerId);

        if (!_employees.Add(new Employee(StoreId, professionalId)))
            throw new DomainException("Professional already works for the store.");
    }

    public void RemoveStaff(int actingOwnerId, int professionalId)
    {
        EnsureOwner(actingOwnerId);

        if (!_employees.Remove(new Employee(StoreId, professionalId)))
            throw new DomainException("Professional does not work for the store.");
    }

    private void EnsureOwner(int professionalId)
    {
        if (!IsOwner(professionalId))
            throw new DomainException("Only an owner can perform this action.");
    }
}