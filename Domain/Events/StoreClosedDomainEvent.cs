using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Events;

public sealed record StoreClosedDomainEvent(
    int StoreId,
    UtcDateTime ClosedAt
) : IDomainEvent;