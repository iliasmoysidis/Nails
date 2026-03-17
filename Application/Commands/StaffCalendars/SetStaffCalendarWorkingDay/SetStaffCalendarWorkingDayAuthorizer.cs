using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StaffCalendars;

public sealed class SetStaffCalendarWorkingDayAuthorizer
    : IAuthorizer<SetStaffCalendarWorkingDayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetStaffCalendarWorkingDayContext _ctx;

    public SetStaffCalendarWorkingDayAuthorizer(
        AuthorizationGuard auth,
        SetStaffCalendarWorkingDayContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetStaffCalendarWorkingDayCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}