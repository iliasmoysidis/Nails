using Application.Schedule.Common.Repositories;
using Domain.Professionals.Events;
using MediatR;

namespace Application.Professionals.DomainEventHandlers;

public sealed class RemoveCalendarsWhenProfessionalLeft
    : INotificationHandler<ProfessionalLeftStoreDomainEvent>
{
    private readonly IProfessionalScheduleRepository _repo;

    public RemoveCalendarsWhenProfessionalLeft(IProfessionalScheduleRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(ProfessionalLeftStoreDomainEvent notification, CancellationToken ct)
    {
        await _repo.RemoveCalendarAsync(
            professionalId: notification.ProfessionalId,
            storeId: notification.StoreId,
            ct: ct
        );
    }
}