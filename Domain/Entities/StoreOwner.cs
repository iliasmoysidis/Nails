using Domain.Common;

namespace Domain.Entities;

public class StoreOwner : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public Store Store { get; private set; } = null!;
    public Professional Professional { get; private set; } = null!;

    private StoreOwner() { }

    public static StoreOwner Create(int storeId, int professionalId)
    {
        return new StoreOwner
        {
            StoreId = storeId,
            ProfessionalId = professionalId
        };
    }
}
