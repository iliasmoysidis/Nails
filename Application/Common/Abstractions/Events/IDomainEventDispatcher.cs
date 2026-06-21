using Domain.Common;

namespace Application.Common.Abstractions.Events;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct);
}