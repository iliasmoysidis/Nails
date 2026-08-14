using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Assignments.Remove;

public sealed class RemoveAssignmentAuthorizer
    : IAuthorizer<RemoveAssignmentCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveAssignmentContext _ctx;

    public RemoveAssignmentAuthorizer(
        AuthorizationGuard auth,
        RemoveAssignmentContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(RemoveAssignmentCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.StoreAssignments.Staff);

        return Task.CompletedTask;
    }
}
