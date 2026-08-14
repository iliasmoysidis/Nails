using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Appointments.MarkNoShow;

public sealed class MarkAppointmentNoShowAuthorizer
    : IAuthorizer<MarkAppointmentNoShowCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly MarkAppointmentNoShowContext _ctx;

    public MarkAppointmentNoShowAuthorizer(
        AuthorizationGuard auth,
        MarkAppointmentNoShowContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(MarkAppointmentNoShowCommand request, CancellationToken ct)
    {
        _auth.EnsureStaffMember(_ctx.Staff);

        return Task.CompletedTask;
    }
}