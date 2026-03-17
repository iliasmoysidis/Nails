using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StoreCalendars;

public sealed class AddStoreCalendarSpecialHoursAuthorizer
    : IAuthorizer<AddStoreCalendarSpecialHoursCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStoreCalendarSpecialHoursContext _ctx;

    public AddStoreCalendarSpecialHoursAuthorizer(
        AuthorizationGuard auth,
        AddStoreCalendarSpecialHoursContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStoreCalendarSpecialHoursCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}