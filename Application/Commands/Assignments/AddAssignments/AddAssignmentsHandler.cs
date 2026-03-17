using MediatR;

namespace Application.Commands.Assignments;

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
        foreach (var offeringId in command.OfferingIds)
        {
            _ctx.Assignments.Add(command.ProfessionalId, offeringId);
        }

        return Task.CompletedTask;
    }
}