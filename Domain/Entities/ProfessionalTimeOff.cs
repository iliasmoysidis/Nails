using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ProfessionalTimeOff : BaseEntity
{
    public int Id { get; private set; }
    public int ProfessionalId { get; private set; }
    public int? StoreId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string? Reason { get; private set; }
    public TimeOffType Type { get; private set; }

    public Professional Professional { get; private set; } = null!;
    public Store? Store { get; private set; }

    private ProfessionalTimeOff() { }
}
