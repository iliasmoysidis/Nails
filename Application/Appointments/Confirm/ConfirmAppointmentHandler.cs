using Domain.Appointments;
using Domain.Common;
using MediatR;

namespace Application.Appointments.Confirm;

public sealed class ConfirmAppointmentHandler : IRequestHandler<ConfirmAppointmentCommand>
{
    private readonly ConfirmAppointmentContext _ctx;
    private readonly IClock _clock;

    public ConfirmAppointmentHandler(
        ConfirmAppointmentContext ctx,
        IClock clock
    )
    {
        _ctx = ctx;
        _clock = clock;
    }

    public Task Handle(ConfirmAppointmentCommand command, CancellationToken ct)
    {
        _ctx.Appointment.Confirm(_clock);

        return Task.CompletedTask;
    }
}
