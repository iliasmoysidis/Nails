using Application.Abstractions.Repositories;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.DomainEventHandlers.Stores;

public sealed class CancelUpcomingAppointmentsWhenStoreClosed
    : INotificationHandler<StoreClosedDomainEvent>
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
        StoreClosedDomainEvent notification,
        CancellationToken ct
    )
    {
        var appointments = await _repo.GetUpcomingByStoreIdAsync(notification.StoreId, ct);

        foreach (var appointment in appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(_clock, "Store has been closed.");
        }
    }
}