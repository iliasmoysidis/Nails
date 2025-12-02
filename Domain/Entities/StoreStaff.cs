using Domain.Exceptions;

namespace Domain.Entities;

public class Staff
{
    public int StoreId { get; private set; }

    private readonly List<StoreOwner> _owners = new();
    public IReadOnlyCollection<StoreOwner> Owners => _owners.AsReadOnly();

    private readonly List<Employee> _employees = new();
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Staff() { }

    public static Staff Create(int storeId)
    {
        return new Staff
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
        return _employees.Any(s => s.ProfessionalId == professionalId);
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

    public Employee AddStaff(int ownerId, int professionalId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can add staff.");
        }

        if (IsStaff(professionalId))
        {
            throw new DomainException("Professional already working for the store.");
        }

        var employee = Employee.Create(StoreId, professionalId);
        _employees.Add(employee);
        return employee;
    }

    public void RemoveStaff(int ownerId, int professionalId)
    {
        if (!IsOwner(ownerId))
        {
            throw new DomainException("Only an owner can remove staff.");
        }

        var employee = _employees.FirstOrDefault(s => s.ProfessionalId == professionalId);

        if (employee == null)
        {
            throw new DomainException("Professional does not work for the store.");
        }

        _employees.Remove(employee);
    }
}