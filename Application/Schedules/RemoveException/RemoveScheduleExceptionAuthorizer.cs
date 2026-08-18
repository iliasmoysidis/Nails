using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Schedules.RemoveException;

public sealed class RemoveScheduleExceptionAuthorizer
    : IAuthorizer<RemoveScheduleExceptionCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveScheduleExceptionContext _ctx;

    public RemoveScheduleExceptionAuthorizer(
        AuthorizationGuard auth,
        RemoveScheduleExceptionContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveScheduleExceptionCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}