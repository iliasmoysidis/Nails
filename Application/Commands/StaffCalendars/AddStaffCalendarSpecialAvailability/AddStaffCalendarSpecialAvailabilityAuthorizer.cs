using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StaffCalendars;

public sealed class AddStaffCalendarSpecialAvailabilityAuthorizer
    : IAuthorizer<AddStaffCalendarSpecialAvailabilityCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStaffCalendarSpecialAvailabilityContext _ctx;

    public AddStaffCalendarSpecialAvailabilityAuthorizer(
        AuthorizationGuard auth,
        AddStaffCalendarSpecialAvailabilityContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStaffCalendarSpecialAvailabilityCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}