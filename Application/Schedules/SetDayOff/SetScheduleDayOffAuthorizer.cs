using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Schedules.SetDayOff;

public sealed class SetScheduleDayOffAuthorizer
    : IAuthorizer<SetScheduleDayOffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetScheduleDayOffContext _ctx;

    public SetScheduleDayOffAuthorizer(
        AuthorizationGuard auth,
        SetScheduleDayOffContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetScheduleDayOffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}