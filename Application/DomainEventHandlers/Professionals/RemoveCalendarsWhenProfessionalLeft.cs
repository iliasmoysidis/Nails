using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Professionals;

public sealed class RemoveCalendarsWhenProfessionalLeft
    : INotificationHandler<ProfessionalLeftStoreDomainEvent>
{
    private readonly IStaffCalendarRepository _repo;

    public RemoveCalendarsWhenProfessionalLeft(IStaffCalendarRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(ProfessionalLeftStoreDomainEvent notification, CancellationToken ct)
    {
        await _repo.RemoveProfessionalAsync(
            storeId: notification.StoreId,
            professionalId: notification.ProfessionalId,
            ct: ct
        );
    }
}