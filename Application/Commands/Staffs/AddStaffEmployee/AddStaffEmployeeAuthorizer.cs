using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Staffs;

public sealed class AddStaffEmployeeAuthorizer
    : IAuthorizer<AddStaffEmployeeCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly AddStaffEmployeeContext _ctx;

    public AddStaffEmployeeAuthorizer(
        AuthorizationGuard auth,
        AddStaffEmployeeContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        AddStaffEmployeeCommand request,
        CancellationToken ct)
    {
        _auth.EnsureOwner(_ctx.Staff);
        return Task.CompletedTask;
    }
}