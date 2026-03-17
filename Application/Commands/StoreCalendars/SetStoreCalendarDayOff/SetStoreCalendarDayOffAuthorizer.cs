using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarDayOffAuthorizer
    : IAuthorizer<SetStoreCalendarDayOffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetStoreCalendarDayOffContext _ctx;

    public SetStoreCalendarDayOffAuthorizer(
        AuthorizationGuard auth,
        SetStoreCalendarDayOffContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetStoreCalendarDayOffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}