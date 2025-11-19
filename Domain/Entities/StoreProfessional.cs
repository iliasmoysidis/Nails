using Domain.Common;

namespace Domain.Entities;

public class StoreProfessional : BaseEntity
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }

    private StoreProfessional() { }

    public static StoreProfessional Create(int storeId, int professionalId)
    {
        return new StoreProfessional
        {
            StoreId = storeId,
            ProfessionalId = professionalId,
        };
    }
}
