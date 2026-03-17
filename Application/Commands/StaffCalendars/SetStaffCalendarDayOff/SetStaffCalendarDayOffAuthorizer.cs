using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarDayOffAuthorizer
    : IAuthorizer<SetStaffCalendarDayOffCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetStaffCalendarDayOffContext _ctx;

    public SetStaffCalendarDayOffAuthorizer(
        AuthorizationGuard auth,
        SetStaffCalendarDayOffContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetStaffCalendarDayOffCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}