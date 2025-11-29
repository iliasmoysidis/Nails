using Domain.Common;

namespace Domain.Entities;

public class StaffService : BaseEntity
{
    public int ProfessionalId { get; private set; }
    public int ServiceId { get; private set; }

    private StaffService() { }

    public static StaffService Create(int professionalId, int serviceId)
    {
        return new StaffService
        {
            ProfessionalId = professionalId,
            ServiceId = serviceId
        };
    }
}
