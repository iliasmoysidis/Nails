using MediatR;

namespace Application.Assignments.Remove;

public sealed class RemoveAssignmentsHandler
    : IRequestHandler<RemoveAssignmentsCommand>
{
    private readonly RemoveAssignmentsContext _ctx;

    public RemoveAssignmentsHandler(RemoveAssignmentsContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(RemoveAssignmentsCommand command, CancellationToken ct)
    {
        _ctx.StoreAssignments.Remove(command.OfferingIds);

        return Task.CompletedTask;
    }
}
