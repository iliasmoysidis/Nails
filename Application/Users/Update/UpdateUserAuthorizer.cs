using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Users.Update;

public sealed class UpdateUserAuthorizer
    : IAuthorizer<UpdateUserCommand>
{
    private readonly AuthorizationGuard _auth;

    public UpdateUserAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(UpdateUserCommand request, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(request.UserId);

        return Task.CompletedTask;
    }
}