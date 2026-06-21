using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Roster.Terminate;

public sealed class Authorizer
    : IAuthorizer<Command>
{
    private readonly AuthorizationGuard _auth;
    private readonly Context _ctx;

    public Authorizer(
        AuthorizationGuard auth,
        Context ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        Command request,
        CancellationToken ct)
    {
        _auth.EnsureOwnerOrSelf(_ctx.Staff, request.ProfessionalId);

        return Task.CompletedTask;
    }
}
