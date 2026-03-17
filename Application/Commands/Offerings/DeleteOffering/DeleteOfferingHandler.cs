using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Offerings;

public sealed class DeleteOfferingHandler
    : IRequestHandler<DeleteOfferingCommand>
{
    private readonly DeleteOfferingContext _ctx;
    private readonly IClock _clock;

    public DeleteOfferingHandler(
        DeleteOfferingContext ctx,
        IClock clock)
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(DeleteOfferingCommand command, CancellationToken ct)
    {
        _ctx.Assignments.RemoveOfferingAssignments(command.OfferingId);

        _ctx.Catalog.RemoveOffering(command.OfferingId, _clock);

        return Task.CompletedTask;
    }
}