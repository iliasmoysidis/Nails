using Domain.Common;

namespace Domain.Entities;

public class ProfessionalSchedule : BaseEntity
{
    public int ProfessionalId { get; private set; }
    public int StoreId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool IsActive { get; private set; } = true;
}
