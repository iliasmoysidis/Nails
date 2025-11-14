namespace Domain.Entities;

public class StoreProfessional
{
    public int StoreId { get; private set; }
    public int ProfessionalId { get; private set; }
    public DateTime StartDate { get; private set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; private set; }
}
