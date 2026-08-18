using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Schedules.AddSpecialAvailability;

public sealed class AddSpecialAvailabilityAuthorizer
    : IAuthorizer<AddSpecialAvailabilityCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddSpecialAvailabilityContext _ctx;

    public AddSpecialAvailabilityAuthorizer(
        AuthorizationGuard auth,
        AddSpecialAvailabilityContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddSpecialAvailabilityCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}