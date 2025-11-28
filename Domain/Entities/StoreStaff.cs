using Domain.Common;

namespace Domain.Entities;

public class StoreStaff : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private StoreStaff() { }

    public static StoreStaff Create(int storeId, int professionalId)
    {
        return new StoreStaff
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
        };
    }
}
