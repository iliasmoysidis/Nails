using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Assignments.Add;

public sealed class AddAssignmentAuthorizer
    : IAuthorizer<AddAssignmentCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddAssignmentContext _ctx;

    public AddAssignmentAuthorizer(
        AuthorizationGuard auth,
        AddAssignmentContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }
    public Task AuthorizeAsync(AddAssignmentCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.StoreAssignments.Staff);

        return Task.CompletedTask;
    }
}
