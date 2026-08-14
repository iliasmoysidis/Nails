using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Calendar.SetDayOff;

public sealed class SetCalendarDayOffAuthorizer
    : IAuthorizer<SetCalendarDayOffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetCalendarDayOffContext _ctx;

    public SetCalendarDayOffAuthorizer(
        AuthorizationGuard auth,
        SetCalendarDayOffContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetCalendarDayOffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}