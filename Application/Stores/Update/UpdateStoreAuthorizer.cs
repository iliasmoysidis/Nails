using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Stores.Update;

public sealed class UpdateStoreAuthorizer
    : IAuthorizer<UpdateStoreCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly UpdateStoreContext _ctx;

    public UpdateStoreAuthorizer(
        AuthorizationGuard auth,
        UpdateStoreContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(UpdateStoreCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}