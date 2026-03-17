using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Staffs;

public sealed class RemoveStaffEmployeeAuthorizer
    : IAuthorizer<RemoveStaffEmployeeCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly RemoveStaffEmployeeContext _ctx;

    public RemoveStaffEmployeeAuthorizer(
        AuthorizationGuard auth,
        RemoveStaffEmployeeContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        RemoveStaffEmployeeCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}