using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Offerings;

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