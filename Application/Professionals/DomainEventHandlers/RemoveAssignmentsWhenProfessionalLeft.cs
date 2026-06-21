using Application.Assignments.Common.Repositories;
using Domain.Professionals.Events;
using MediatR;

namespace Application.Professionals.DomainEventHandlers;

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