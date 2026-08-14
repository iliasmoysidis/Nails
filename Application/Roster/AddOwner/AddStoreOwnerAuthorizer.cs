using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Roster.AddOwner;

public sealed class AddStoreOwnerAuthorizer
    : IAuthorizer<AddStoreOwnerCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStoreOwnerContext _ctx;

    public AddStoreOwnerAuthorizer(
        AuthorizationGuard auth,
        AddStoreOwnerContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStoreOwnerCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}