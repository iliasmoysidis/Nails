using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Catalog.Create;

public sealed class CreateOfferingAuthorizer
    : IAuthorizer<CreateOfferingCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly CreateOfferingContext _ctx;

    public CreateOfferingAuthorizer(
        AuthorizationGuard auth,
        CreateOfferingContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(CreateOfferingCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);

        return Task.CompletedTask;
    }
}