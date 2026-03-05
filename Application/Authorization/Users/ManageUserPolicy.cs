using Application.Abstractions.Policies.Users;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Users;

public sealed class ManageUserPolicy : IManageUserPolicy
{
    private readonly IRequestContext _context;

    public ManageUserPolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanManage(int userId)
    {
        if (!_context.IsUser || _context.ActorId != userId) throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Users can only manage their own account.");
}