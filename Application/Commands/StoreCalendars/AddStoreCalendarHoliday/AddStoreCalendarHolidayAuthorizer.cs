using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StoreCalendars;

public sealed class AddStoreCalendarHolidayAuthorizer
    : IAuthorizer<AddStoreCalendarHolidayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStoreCalendarHolidayContext _ctx;

    public AddStoreCalendarHolidayAuthorizer(
        AuthorizationGuard auth,
        AddStoreCalendarHolidayContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStoreCalendarHolidayCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}