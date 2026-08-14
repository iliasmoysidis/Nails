using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Roster.Terminate;

public sealed class TerminateStaffAuthorizer
    : IAuthorizer<TerminateStaffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly TerminateStaffContext _ctx;

    public TerminateStaffAuthorizer(
        AuthorizationGuard auth,
        TerminateStaffContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        TerminateStaffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwnerOrSelf(_ctx.Staff, request.ProfessionalId);

        return Task.CompletedTask;
    }
}
