using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class RemoveCalendarsWhenStoreClosed
    : INotificationHandler<StoreClosedDomainEvent>
{
    private readonly IStoreCalendarRepository _storeCalendarRepo;
    private readonly IStaffCalendarRepository _staffCalendarRepo;

    public RemoveCalendarsWhenStoreClosed(
        IStoreCalendarRepository storeCalendarRepo,
        IStaffCalendarRepository staffCalendarRepo
    )
    {
        _storeCalendarRepo = storeCalendarRepo;
        _staffCalendarRepo = staffCalendarRepo;
    }

    public async Task Handle(
        StoreClosedDomainEvent notification,
        CancellationToken ct
    )
    {
        var storeId = notification.StoreId;

        await _storeCalendarRepo.RemoveAsync(storeId, ct);
        await _staffCalendarRepo.RemoveAsync(storeId, ct);
    }
}