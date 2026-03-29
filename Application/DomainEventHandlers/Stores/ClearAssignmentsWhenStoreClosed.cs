using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearAssignmentsWhenStoreClosed
    : INotificationHandler<StoreClosedDomainEvent>
{
    private readonly IAssignmentsRepository _repo;

    public ClearAssignmentsWhenStoreClosed(IAssignmentsRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        StoreClosedDomainEvent notification,
        CancellationToken ct
    )
    {
        var assignments = await _repo.GetByStoreIdAsync(notification.StoreId, ct);

        assignments?.Clear();
    }
}