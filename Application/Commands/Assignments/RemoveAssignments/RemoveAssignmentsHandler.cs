using MediatR;

namespace Application.Commands.Assignments;

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
        foreach (var offeringId in command.OfferingIds)
        {
            _ctx.Assignments.Remove(command.ProfessionalId, offeringId);
        }

        return Task.CompletedTask;
    }
}