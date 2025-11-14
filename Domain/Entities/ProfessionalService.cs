namespace Domain.Entities;

public class ProfessionalService
{
    public int ProfessionalId { get; private set; }
    public int ServiceId { get; private set; }

    public User Professional { get; private set; } = null!;
    public Service Service { get; private set; } = null!;

    private ProfessionalService() { }
}
