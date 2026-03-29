using MediatR;

namespace Application.Features.Assignments.Add;

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
        foreach (var offeringId in command.OfferingIds)
        {
            _ctx.Assignments.Add(command.ProfessionalId, offeringId);
        }

        return Task.CompletedTask;
    }
}