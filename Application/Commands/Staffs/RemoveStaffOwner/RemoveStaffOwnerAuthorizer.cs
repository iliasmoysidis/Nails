using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffOwnerAuthorizer
    : IAuthorizer<RemoveStaffOwnerCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveStaffOwnerContext _ctx;

    public RemoveStaffOwnerAuthorizer(
        AuthorizationGuard auth,
        RemoveStaffOwnerContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveStaffOwnerCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}