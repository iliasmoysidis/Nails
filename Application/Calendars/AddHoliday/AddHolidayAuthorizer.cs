using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Calendars.AddHoliday;

public sealed class AddHolidayAuthorizer
    : IAuthorizer<AddHolidayCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddHolidayContext _ctx;

    public AddHolidayAuthorizer(
        AuthorizationGuard auth,
        AddHolidayContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddHolidayCommand request,
        CancellationToken ct
    )
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}