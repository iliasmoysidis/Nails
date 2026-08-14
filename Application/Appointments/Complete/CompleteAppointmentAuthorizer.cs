using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Appointments.Complete;

public sealed class CompleteAppointmentAuthorizer
    : IAuthorizer<CompleteAppointmentCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly CompleteAppointmentContext _ctx;

    public CompleteAppointmentAuthorizer(
        AuthorizationGuard auth,
        CompleteAppointmentContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(CompleteAppointmentCommand request, CancellationToken ct)
    {
        _auth.EnsureStaffMember(_ctx.Staff);

        return Task.CompletedTask;
    }
}