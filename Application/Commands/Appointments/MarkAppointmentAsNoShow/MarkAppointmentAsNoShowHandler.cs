using Domain.Interfaces;
using MediatR;

namespace Application.Commands.Appointments;

public sealed class MarkAppointmentAsNoShowHandler
    : IRequestHandler<MarkAppointmentAsNoShowCommand>
{
    private readonly MarkAppointmentAsNoShowContext _ctx;

    private readonly IClock _clock;

    public MarkAppointmentAsNoShowHandler(
        MarkAppointmentAsNoShowContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(MarkAppointmentAsNoShowCommand command, CancellationToken ct)
    {
        _ctx.Appointment.MarkAsNoShow(_clock);

        return Task.CompletedTask;
    }
}