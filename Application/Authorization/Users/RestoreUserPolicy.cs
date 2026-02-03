using Application.Abstractions.Policies.Users;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Commands.Users;

public sealed class RestoreUserPolicy : IRestoreUserPolicy
{
    private readonly IRequestContext _context;

    public RestoreUserPolicy(IRequestContext context)
    {
        _context = context;
    }

    public Task EnsureCanRestoreAsync(int userId, CancellationToken ct)
    {
        if (!_context.IsUser || _context.ActorId == userId) throw Forbidden();

        return Task.CompletedTask;
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Users can only restore their own account.");
}