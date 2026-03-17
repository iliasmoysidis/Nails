using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.StoreCalendars;

public sealed class RemoveStoreCalendarExceptionAuthorizer
    : IAuthorizer<RemoveStoreCalendarExceptionCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveStoreCalendarExceptionContext _ctx;

    public RemoveStoreCalendarExceptionAuthorizer(
        AuthorizationGuard auth,
        RemoveStoreCalendarExceptionContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveStoreCalendarExceptionCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}