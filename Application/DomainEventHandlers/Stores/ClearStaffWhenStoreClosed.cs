using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class ClearStaffWhenStoreClosed
    : INotificationHandler<StoreClosedDomainEvent>
{
    private readonly IStaffRepository _repo;

    public ClearStaffWhenStoreClosed(IStaffRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(
        StoreClosedDomainEvent notification,
        CancellationToken ct
    )
    {
        var staff = await _repo.GetByStoreIdAsync(notification.StoreId, ct);

        staff?.Clear();
    }
}