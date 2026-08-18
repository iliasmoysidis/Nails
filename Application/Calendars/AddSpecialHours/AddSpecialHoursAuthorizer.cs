using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Calendars.AddSpecialHours;

public sealed class AddSpecialHoursAuthorizer
    : IAuthorizer<AddSpecialHoursCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddSpecialHoursContext _ctx;

    public AddSpecialHoursAuthorizer(
        AuthorizationGuard auth,
        AddSpecialHoursContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddSpecialHoursCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}