using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Stores.Close;

public sealed class CloseStoreAuthorizer
    : IAuthorizer<CloseStoreCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly CloseStoreContext _ctx;

    public CloseStoreAuthorizer(
        AuthorizationGuard auth,
        CloseStoreContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(CloseStoreCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}
