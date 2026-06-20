using Domain.Professionals.Events;
using Domain.Professionals;
using Domain.Common;
using Application.Abstractions.Repositories;
using MediatR;

namespace Application.DomainEventHandlers.Professionals;

public sealed class CancelAppointmentsWhenProfessionalLeft
    : INotificationHandler<ProfessionalLeftStoreDomainEvent>
{
    private readonly IAppointmentRepository _repo;
    private readonly IClock _clock;

    public CancelAppointmentsWhenProfessionalLeft(
        IAppointmentRepository repo,
        IClock clock
    )
    {
        _repo = repo;
        _clock = clock;
    }

    public async Task Handle(ProfessionalLeftStoreDomainEvent notification, CancellationToken ct)
    {
        var appointments = await _repo.GetUpcomingByStoreIdAndProfessionalId(
            storeId: notification.StoreId,
            professionalId: notification.ProfessionalId,
            ct: ct
        );

        foreach (var appointment in appointments)
        {
            if (appointment.IsTerminal)
                continue;

            appointment.Cancel(_clock, "Professional left the store.");
        }
    }
}