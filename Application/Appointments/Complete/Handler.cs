using Domain.Appointments;
using Domain.Common;
using MediatR;

namespace Application.Appointments.Complete;

public sealed class Handler
    : IRequestHandler<Command>
{
    private readonly Context _ctx;
    private readonly IClock _clock;

    public Handler(
        Context ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(Command command, CancellationToken ct)
    {
        _ctx.Appointment.Complete(_clock);

        return Task.CompletedTask;
    }
}