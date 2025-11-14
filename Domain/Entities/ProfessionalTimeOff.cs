using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ProfessionalTimeOff : BaseEntity
{
    public int ProfessionalId { get; private set; }
    public int? StoreId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string? Reason { get; private set; }
    public TimeOffType Type { get; private set; }
}
