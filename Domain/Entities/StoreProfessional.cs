namespace Domain.Entities;

public class StoreProfessional
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }

    public Store Store { get; private set; } = null!;
    public User Professional { get; private set; } = null!;

    private StoreProfessional()
    {
        StartDate = DateTime.UtcNow;
    }
}
