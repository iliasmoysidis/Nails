using MediatR;

namespace Application.Features.Assignments.Remove;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(Command command, CancellationToken ct)
    {
        _ctx.StoreAssignments.Remove(command.ProfessionalId, command.OfferingIds);

        return Task.CompletedTask;
    }
}
