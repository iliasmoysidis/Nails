using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Appointments;

public sealed class CompleteAppointmentHandler
    : IRequestHandler<CompleteAppointmentCommand>
{
    private readonly CompleteAppointmentContext _ctx;
    private readonly IClock _clock;

    public CompleteAppointmentHandler(
        CompleteAppointmentContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(CompleteAppointmentCommand command, CancellationToken ct)
    {
        _ctx.Appointment.Complete(_clock);

        return Task.CompletedTask;
    }
}