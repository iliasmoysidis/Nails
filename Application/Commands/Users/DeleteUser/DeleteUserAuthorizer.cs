using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Users;

public sealed class DeleteUserAuthorizer
    : IAuthorizer<DeleteUserCommand>
{
    private readonly AuthorizationGuard _auth;

    public DeleteUserAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(DeleteUserCommand request, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(request.UserId);

        return Task.CompletedTask;
    }
}