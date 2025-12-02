using Domain.Common;

namespace Domain.Entities;

public class Employee : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private Employee() { }

    public static Employee Create(int storeId, int professionalId)
    {
        return new Employee
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
        };
    }
}
