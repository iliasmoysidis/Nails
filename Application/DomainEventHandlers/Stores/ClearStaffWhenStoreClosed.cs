using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearStaffWhenStoreClosed
    : INotificationHandler<DomainEventNotification<StoreClosedDomainEvent>>
{
    private readonly IStaffRepository _repo;

    public ClearStaffWhenStoreClosed(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        DomainEventNotification<StoreClosedDomainEvent> notification,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(notification.DomainEvent.StoreId, ct);

        staff?.Clear();
    }
}