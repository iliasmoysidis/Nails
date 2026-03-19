using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearAssignmentsWhenStoreClosed
    : INotificationHandler<DomainEventNotification<StoreClosedDomainEvent>>
{
    private readonly IAssignmentsRepository _repo;

    public ClearAssignmentsWhenStoreClosed(IAssignmentsRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        DomainEventNotification<StoreClosedDomainEvent> notification,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByStoreIdAsync(notification.DomainEvent.StoreId, ct);

        assignments?.Clear();
    }
}