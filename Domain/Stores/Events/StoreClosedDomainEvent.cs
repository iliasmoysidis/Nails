using Domain.Common;
using Domain.Common.ValueObjects;

namespace Domain.Stores.Events;

public sealed record StoreClosedDomainEvent(
    int StoreId,
    UtcDateTime ClosedAt
) : IDomainEvent;