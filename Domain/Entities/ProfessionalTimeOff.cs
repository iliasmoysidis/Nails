using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class ProfessionalTimeOff : BaseEntity
{
    public int Id { get; private set; }
    public int ProfessionalId { get; private set; }
    public int? StoreId { get; private set; }
    public DateTime StartAt { get; private set; }
    public DateTime EndAt { get; private set; }
    public string? Reason { get; private set; }
    public TimeOffType? Type { get; private set; }

    public Professional Professional { get; private set; } = null!;
    public Store? Store { get; private set; }

    private ProfessionalTimeOff() { }

    public static ProfessionalTimeOff Create(int profesionalId, DateTime startAt, DateTime endAt, TimeOffType? type = null, string? reason = null, int? storeId = null)
    {
        if (startAt >= endAt)
        {
            throw new DomainException("Start date must be before end date.");
        }

        return new ProfessionalTimeOff
        {
            ProfessionalId = profesionalId,
            StoreId = storeId,
            StartAt = startAt,
            EndAt = endAt,
            Reason = reason,
            Type = type
        };
    }
}
