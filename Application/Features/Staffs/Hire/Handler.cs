using MediatR;

namespace Application.Features.Staffs.Hire;

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
        _ctx.EmploymentCreation.Hire();

        return Task.CompletedTask;
    }
}
