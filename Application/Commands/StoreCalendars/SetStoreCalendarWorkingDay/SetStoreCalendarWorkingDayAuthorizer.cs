using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StoreCalendars;

public sealed class SetStoreCalendarWorkingDayAuthorizer
    : IAuthorizer<SetStoreCalendarWorkingDayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetStoreCalendarWorkingDayContext _ctx;

    public SetStoreCalendarWorkingDayAuthorizer(
        AuthorizationGuard auth,
        SetStoreCalendarWorkingDayContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetStoreCalendarWorkingDayCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}