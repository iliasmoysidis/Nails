using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarVacationAuthorizer
    : IAuthorizer<AddStaffCalendarVacationCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStaffCalendarVacationContext _ctx;

    public AddStaffCalendarVacationAuthorizer(
        AuthorizationGuard auth,
        AddStaffCalendarVacationContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStaffCalendarVacationCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}