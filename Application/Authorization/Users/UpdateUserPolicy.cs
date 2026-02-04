using Application.Abstractions.Policies.Users;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Users;

public sealed class UpdateUserPolicy : IUpdateUserPolicy
{
    private readonly IRequestContext _context;

    public UpdateUserPolicy(IRequestContext context)
    {
        _context = context;
    }

    public Task EnsureCanUpdateAsync(int userId, CancellationToken ct)
    {
        if (!_context.IsUser || _context.ActorId == userId) throw Forbidden();

        return Task.CompletedTask;
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Users can only update their own profile.");
}