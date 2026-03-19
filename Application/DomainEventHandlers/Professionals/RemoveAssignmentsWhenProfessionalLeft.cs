using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Professionals;

public sealed class RemoveAssignmentsWhenProfessionalLeft
    : INotificationHandler<DomainEventNotification<ProfessionalLeftStoreDomainEvent>>
{
    private readonly IAssignmentsRepository _repo;

    public RemoveAssignmentsWhenProfessionalLeft(IAssignmentsRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DomainEventNotification<ProfessionalLeftStoreDomainEvent> notification, CancellationToken ct)
    {
        var assignments = await _repo.GetByStoreIdAsync(notification.DomainEvent.StoreId, ct);

        assignments?.RemoveByProfessional(notification.DomainEvent.ProfessionalId);
    }
}