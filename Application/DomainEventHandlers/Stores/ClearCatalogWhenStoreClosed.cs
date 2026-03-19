using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearCatalogWhenStoreClosed
    : INotificationHandler<DomainEventNotification<StoreClosedDomainEvent>>
{
    private readonly IStoreCatalogRepository _repo;

    public ClearCatalogWhenStoreClosed(IStoreCatalogRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        DomainEventNotification<StoreClosedDomainEvent> notification,
        CancellationToken ct
    )
    {
        var catalog = await _repo.GetByIdAsync(notification.DomainEvent.StoreId, ct);

        catalog?.Clear();
    }
}