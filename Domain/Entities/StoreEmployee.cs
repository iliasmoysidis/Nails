using Domain.Common;

namespace Domain.Entities;

public class StoreEmployee : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private StoreEmployee() { }

    public static StoreEmployee Create(int storeId, int professionalId)
    {
        return new StoreEmployee
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
        };
    }
}
