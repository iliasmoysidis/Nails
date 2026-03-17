using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Appointments;

public sealed class CancelAppointmentAuthorizer
    : IAuthorizer<CancelAppointmentCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly CancelAppointmentContext _ctx;

    public CancelAppointmentAuthorizer(
        AuthorizationGuard auth,
        CancelAppointmentContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(CancelAppointmentCommand request, CancellationToken ct)
    {
        _auth.EnsureCanModifyAppointment(_ctx.Appointment, _ctx.Staff);

        return Task.CompletedTask;
    }
}