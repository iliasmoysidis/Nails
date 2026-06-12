using MediatR;

namespace Application.Features.Offerings.Delete;

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
        _ctx.Assignments.RemoveByOffering(command.OfferingId);

        _ctx.Catalog.RemoveOffering(command.OfferingId);

        return Task.CompletedTask;
    }
}