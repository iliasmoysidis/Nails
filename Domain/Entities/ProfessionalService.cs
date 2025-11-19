using Domain.Common;

namespace Domain.Entities;

public class ProfessionalService : BaseEntity
{
    public int ProfessionalId { get; private set; }
    public int ServiceId { get; private set; }

    private ProfessionalService() { }

    public static ProfessionalService Create(int professionalId, int serviceId)
    {
        return new ProfessionalService
        {
            ProfessionalId = professionalId,
            ServiceId = serviceId
        };
    }
}
