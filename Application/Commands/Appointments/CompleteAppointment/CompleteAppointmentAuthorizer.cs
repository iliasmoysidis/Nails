using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Appointments;

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