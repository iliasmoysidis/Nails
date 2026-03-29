using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearCatalogWhenStoreClosed
    : INotificationHandler<StoreClosedDomainEvent>
{
    private readonly IStoreCatalogRepository _repo;

    public ClearCatalogWhenStoreClosed(IStoreCatalogRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        StoreClosedDomainEvent notification,
        CancellationToken ct
    )
    {
        var catalog = await _repo.GetByIdAsync(notification.StoreId, ct);

        catalog?.Clear();
    }
}