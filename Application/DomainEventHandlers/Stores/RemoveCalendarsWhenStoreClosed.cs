using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class RemoveCalendarsWhenStoreClosed
    : INotificationHandler<DomainEventNotification<StoreClosedDomainEvent>>
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
        DomainEventNotification<StoreClosedDomainEvent> notification,
        CancellationToken ct
    )
    {
        var storeId = notification.DomainEvent.StoreId;

        await _storeCalendarRepo.RemoveAsync(storeId, ct);
        await _staffCalendarRepo.RemoveAsync(storeId, ct);
    }
}