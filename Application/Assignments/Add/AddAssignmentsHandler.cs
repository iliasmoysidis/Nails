using MediatR;

namespace Application.Assignments.Add;

public sealed class AddAssignmentsHandler
    : IRequestHandler<AddAssignmentsCommand>
{
    private readonly AddAssignmentsContext _ctx;

    public AddAssignmentsHandler(AddAssignmentsContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(AddAssignmentsCommand command, CancellationToken ct)
    {
        _ctx.StoreAssignments.Assign(command.OfferingIds);

        return Task.CompletedTask;
    }
}
