using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Assignments.Add;

public sealed class AddAssignmentsAuthorizer
    : IAuthorizer<AddAssignmentsCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddAssignmentsContext _ctx;

    public AddAssignmentsAuthorizer(
        AuthorizationGuard auth,
        AddAssignmentsContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }
    public Task AuthorizeAsync(AddAssignmentsCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);

        return Task.CompletedTask;
    }
}
