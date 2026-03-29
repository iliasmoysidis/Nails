using Application.Abstractions.Repositories;
using Domain.Events;
using MediatR;

namespace Application.DomainEventHandlers.Professionals;

public sealed class RemoveAssignmentsWhenProfessionalLeft
    : INotificationHandler<ProfessionalLeftStoreDomainEvent>
{
    private readonly IAssignmentsRepository _repo;

    public RemoveAssignmentsWhenProfessionalLeft(IAssignmentsRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(ProfessionalLeftStoreDomainEvent notification, CancellationToken ct)
    {
        var assignments = await _repo.GetByStoreIdAsync(notification.StoreId, ct);

        assignments?.RemoveByProfessional(notification.ProfessionalId);
    }
}