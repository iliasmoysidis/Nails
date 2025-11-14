using Domain.Common;

namespace Domain.Entities;

public class ProfessionalSchedule : BaseEntity
{
    public int Id { get; private set; }
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }

    public Professional Professional { get; set; } = null!;
    public Store Store { get; set; } = null!;

    private ProfessionalSchedule() { }
}
