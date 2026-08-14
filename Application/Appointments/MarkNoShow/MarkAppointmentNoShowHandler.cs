using Domain.Appointments;
using Domain.Common;
using MediatR;

namespace Application.Appointments.MarkNoShow;

public sealed class MarkAppointmentNoShowHandler
    : IRequestHandler<MarkAppointmentNoShowCommand>
{
    private readonly MarkAppointmentNoShowContext _ctx;

    private readonly IClock _clock;

    public MarkAppointmentNoShowHandler(
        MarkAppointmentNoShowContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(MarkAppointmentNoShowCommand command, CancellationToken ct)
    {
        _ctx.Appointment.MarkAsNoShow(_clock);

        return Task.CompletedTask;
    }
}