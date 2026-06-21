using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Assignments.Remove;

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
        _auth.EnsureOwner(_ctx.StoreAssignments.Staff);

        return Task.CompletedTask;
    }
}
