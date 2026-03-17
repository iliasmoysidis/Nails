using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Appointments;

public sealed class MarkAppointmentAsNoShowAuthorizer
    : IAuthorizer<MarkAppointmentAsNoShowCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly MarkAppointmentAsNoShowContext _ctx;

    public MarkAppointmentAsNoShowAuthorizer(
        AuthorizationGuard auth,
        MarkAppointmentAsNoShowContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(MarkAppointmentAsNoShowCommand request, CancellationToken ct)
    {
        _auth.EnsureStaffMember(_ctx.Staff);

        return Task.CompletedTask;
    }
}