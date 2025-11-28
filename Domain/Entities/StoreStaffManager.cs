using Domain.Exceptions;

namespace Domain.Entities;

public class StoreStaffManager
{
    public int StoreId { get; private set; }

    private readonly List<StoreProfessional> _staff = new();
    public IReadOnlyCollection<StoreProfessional> Staff => _staff.AsReadOnly();

    public bool IsStaff(int professionalId)
    {
        return _staff.Any(s => s.ProfessionalId == professionalId);
    }


    public StoreProfessional AddStaff(int professionalId)
    {
        if (IsStaff(professionalId))
        {
            throw new DomainException("Professional already working for the store.");
        }

        var employee = StoreProfessional.Create(StoreId, professionalId);
        _staff.Add(employee);
        return employee;
    }

    public void RemoveStaff(int professionalId)
    {
        var employee = _staff.FirstOrDefault(s => s.ProfessionalId == professionalId);

        if (employee == null)
        {
            throw new DomainException("Professional does not work for the store.");
        }

        _staff.Remove(employee);
    }
}