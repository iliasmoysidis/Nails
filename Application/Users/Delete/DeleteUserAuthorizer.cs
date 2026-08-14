using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Users.Delete;

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