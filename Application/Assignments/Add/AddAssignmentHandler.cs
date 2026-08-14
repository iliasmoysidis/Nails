using MediatR;

namespace Application.Assignments.Add;

public sealed class AddAssignmentHandler
    : IRequestHandler<AddAssignmentCommand>
{
    private readonly AddAssignmentContext _ctx;

    public AddAssignmentHandler(AddAssignmentContext ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(AddAssignmentCommand command, CancellationToken ct)
    {
        _ctx.StoreAssignments.Assign(command.OfferingIds);

        return Task.CompletedTask;
    }
}
