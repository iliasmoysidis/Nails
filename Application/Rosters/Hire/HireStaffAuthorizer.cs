using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Rosters.Hire;

public sealed class HireStaffAuthorizer
    : IAuthorizer<HireStaffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly HireStaffContext _ctx;

    public HireStaffAuthorizer(
        AuthorizationGuard auth,
        HireStaffContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        HireStaffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}
