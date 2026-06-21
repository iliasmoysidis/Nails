using MediatR;

namespace Application.Schedule.RemoveException;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;

    public Handler(
        Context ctx)
    {
        _ctx = ctx;
    }

    public Task Handle(
        Command command,
        CancellationToken ct)
    {
        _ctx.ProfessionalAvailability.RemoveException(command.Date);

        return Task.CompletedTask;
    }
}
