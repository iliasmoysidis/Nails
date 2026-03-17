using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StaffCalendars;

public sealed class RemoveStaffCalendarExceptionAuthorizer
    : IAuthorizer<RemoveStaffCalendarExceptionCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveStaffCalendarExceptionContext _ctx;

    public RemoveStaffCalendarExceptionAuthorizer(
        AuthorizationGuard auth,
        RemoveStaffCalendarExceptionContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveStaffCalendarExceptionCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}