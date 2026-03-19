using Application.Abstractions.Repositories;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class CancelUpcomingAppointmentsWhenStoreClosed
    : INotificationHandler<DomainEventNotification<StoreClosedDomainEvent>>
{
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;

    public CancelUpcomingAppointmentsWhenStoreClosed(
        IAppointmentRepository repo,
        IClock clock
    )
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task Handle(
        DomainEventNotification<StoreClosedDomainEvent> notification,
        CancellationToken ct
    )
    {
        var domainEvent = notification.DomainEvent;

        var appointments = await _repo.GetUpcomingByStoreIdAsync(domainEvent.StoreId, ct);

        foreach (var appointment in appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(_clock, "Store has been closed.");
        }
    }
}