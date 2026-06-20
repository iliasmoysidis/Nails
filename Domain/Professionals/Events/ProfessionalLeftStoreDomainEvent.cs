using Domain.Common;
using Domain.Common.ValueObjects;

namespace Domain.Professionals.Events;

public sealed record ProfessionalLeftStoreDomainEvent(
    int StoreId,
    int ProfessionalId,
    UtcDateTime OccurredAt
) : IDomainEvent;