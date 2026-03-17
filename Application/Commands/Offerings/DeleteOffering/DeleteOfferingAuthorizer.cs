using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Offerings;

public sealed class DeleteOfferingAuthorizer
    : IAuthorizer<DeleteOfferingCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly DeleteOfferingContext _ctx;

    public DeleteOfferingAuthorizer(
        AuthorizationGuard auth,
        DeleteOfferingContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(DeleteOfferingCommand request, CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}