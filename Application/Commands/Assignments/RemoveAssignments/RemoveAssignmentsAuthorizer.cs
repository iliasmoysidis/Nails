using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Assignments;

public sealed class RemoveAssignmentsAuthorizer
    : IAuthorizer<RemoveAssignmentsCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveAssignmentsContext _ctx;

    public RemoveAssignmentsAuthorizer(
        AuthorizationGuard auth,
        RemoveAssignmentsContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(RemoveAssignmentsCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);

        return Task.CompletedTask;
    }
}