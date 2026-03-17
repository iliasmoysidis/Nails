using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Appointments;

public sealed class CancelAppointmentHandler
    : IRequestHandler<CancelAppointmentCommand>
{
    private readonly CancelAppointmentContext _ctx;
    private readonly IClock _clock;

    public CancelAppointmentHandler(
        CancelAppointmentContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(CancelAppointmentCommand command, CancellationToken ct)
    {
        _ctx.Appointment.Cancel(_clock, command.Reason);

        return Task.CompletedTask;
    }
}