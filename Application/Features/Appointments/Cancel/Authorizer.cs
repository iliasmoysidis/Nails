using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Features.Appointments.Cancel;

public sealed class Authorizer
    : IAuthorizer<Command>
{
    private readonly AuthorizationGuard _auth;
    private readonly Context _ctx;

    public Authorizer(
        AuthorizationGuard auth,
        Context ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(Command request, CancellationToken ct)
    {
        _auth.EnsureCanModifyAppointment(_ctx.Appointment, _ctx.Staff);

        return Task.CompletedTask;
    }
}