using MediatR;

namespace Application.Features.Staffs.RemoveOwner;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct)
    {
        _ctx.Staff.RemoveOwner(command.ProfessionalId);

        return Task.CompletedTask;
    }
}