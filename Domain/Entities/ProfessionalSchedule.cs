namespace Domain.Entities;

public class ProfessionalSchedule
{
    public int Id { get; set; }
    public int ProfessionalId { get; set; }
    public int StoreId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; } = true;

    public User Professional { get; set; } = null!;
    public Store Store { get; set; } = null!;
}
