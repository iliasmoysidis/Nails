using MediatR;

namespace Application.Assignments.Remove;

public sealed class RemoveAssignmentHandler
    : IRequestHandler<RemoveAssignmentCommand>
{
    private readonly RemoveAssignmentContext _ctx;

    public RemoveAssignmentHandler(RemoveAssignmentContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(RemoveAssignmentCommand command, CancellationToken ct)
    {
        _ctx.StoreAssignments.Remove(command.OfferingIds);

        return Task.CompletedTask;
    }
}
