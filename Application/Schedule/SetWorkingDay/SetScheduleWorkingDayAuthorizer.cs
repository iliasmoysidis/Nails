using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Schedule.SetWorkingDay;

public sealed class SetScheduleWorkingDayAuthorizer
    : IAuthorizer<SetScheduleWorkingDayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly SetScheduleWorkingDayContext _ctx;

    public SetScheduleWorkingDayAuthorizer(
        AuthorizationGuard auth,
        SetScheduleWorkingDayContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        SetScheduleWorkingDayCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}