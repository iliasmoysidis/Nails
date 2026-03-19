using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Events;

public sealed record ProfessionalLeftStoreDomainEvent(
    int StoreId,
    int ProfessionalId,
    UtcDateTime OccurredAt
) : IDomainEvent;