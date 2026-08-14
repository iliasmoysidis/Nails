using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Catalog.Remove;

public sealed class RemoveOfferingAuthorizer
    : IAuthorizer<RemoveOfferingCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveOfferingContext _ctx;

    public RemoveOfferingAuthorizer(
        AuthorizationGuard auth,
        RemoveOfferingContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(RemoveOfferingCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}