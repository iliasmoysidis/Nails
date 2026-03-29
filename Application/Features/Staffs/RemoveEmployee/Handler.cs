using MediatR;

namespace Application.Features.Staffs.RemoveEmployee;

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
        _ctx.Assignments.RemoveByProfessional(command.ProfessionalId);

        _ctx.Staff.RemoveEmployee(command.ProfessionalId);

        return Task.CompletedTask;
    }
}