using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Calendar.SetWorkingDay;

public sealed class SetCalendarWorkingDayAuthorizer
    : IAuthorizer<SetCalendarWorkingDayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetCalendarWorkingDayContext _ctx;

    public SetCalendarWorkingDayAuthorizer(
        AuthorizationGuard auth,
        SetCalendarWorkingDayContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetCalendarWorkingDayCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}