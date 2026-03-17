using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Offerings;

public sealed class UpdateOfferingAuthorizer
    : IAuthorizer<UpdateOfferingCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly UpdateOfferingContext _ctx;

    public UpdateOfferingAuthorizer(
        AuthorizationGuard auth,
        UpdateOfferingContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(UpdateOfferingCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}