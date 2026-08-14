using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Appointments.Confirm;

public sealed class ConfirmAppointmentAuthorizer
    : IAuthorizer<ConfirmAppointmentCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly ConfirmAppointmentContext _ctx;

    public ConfirmAppointmentAuthorizer(
        AuthorizationGuard auth,
        ConfirmAppointmentContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(ConfirmAppointmentCommand request, CancellationToken ct)
    {
        _auth.EnsureStaffMember(_ctx.Staff);

        return Task.CompletedTask;
    }
}