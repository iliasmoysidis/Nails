using MediatR;

namespace Application.Features.Staffs.AddEmployee;

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
        CancellationToken ct
    )
    {
        _ctx.Staff.AddEmployee(command.ProfessionalId);

        return Task.CompletedTask;
    }
}