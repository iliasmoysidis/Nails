using Domain.Common;

namespace Domain.Entities;

public class ProfessionalService : BaseEntity
{
    public int ProfessionalId { get; private set; }
    public int ServiceId { get; private set; }

    public Professional Professional { get; private set; } = null!;
    public Service Service { get; private set; } = null!;

    private ProfessionalService() { }
}
