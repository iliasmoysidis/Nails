using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Calendars.RemoveException;

public sealed class RemoveCalendarExceptionAuthorizer
    : IAuthorizer<RemoveCalendarExceptionCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveCalendarExceptionContext _ctx;

    public RemoveCalendarExceptionAuthorizer(
        AuthorizationGuard auth,
        RemoveCalendarExceptionContext ctx
    )
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveCalendarExceptionCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}