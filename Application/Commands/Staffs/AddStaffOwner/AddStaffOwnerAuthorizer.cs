using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Staffs;

public sealed class AddStaffOwnerAuthorizer
    : IAuthorizer<AddStaffOwnerCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStaffOwnerContext _ctx;

    public AddStaffOwnerAuthorizer(
        AuthorizationGuard auth,
        AddStaffOwnerContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStaffOwnerCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}