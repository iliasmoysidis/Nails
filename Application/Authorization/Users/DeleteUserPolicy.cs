using Application.Abstractions.Policies.Users;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Users;

public sealed class DeleteUserPolicy : IDeleteUserPolicy
{
    private readonly IRequestContext _context;

    public DeleteUserPolicy(IRequestContext context)
    {
        _context = context;
    }

    public Task EnsureCanDeleteAsync(int userId, CancellationToken ct)
    {
        if (!_context.IsUser || _context.ActorId != userId) throw Forbidden();

        return Task.CompletedTask;
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Users can only delete their own account.");
}