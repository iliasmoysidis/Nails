using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Stores;

public sealed class CloseStoreAuthorizer
    : IAuthorizer<CloseStoreCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly CloseStoreContext _ctx;

    public CloseStoreAuthorizer(
        AuthorizationGuard auth,
        CloseStoreContext ctx)
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