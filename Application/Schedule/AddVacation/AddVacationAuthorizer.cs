using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Schedule.AddVacation;

public sealed class AddVacationAuthorizer
    : IAuthorizer<AddVacationCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddVacationContext _ctx;

    public AddVacationAuthorizer(
        AuthorizationGuard auth,
        AddVacationContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddVacationCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}