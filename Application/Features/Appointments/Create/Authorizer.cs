using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Features.Appointments.Create;

public sealed class Authorizer
    : IAuthorizer<Command>
{
    private readonly AuthorizationGuard _auth;

    public Authorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(Command request, CancellationToken ct)
    {
        _auth.EnsureUser();
        _auth.EnsureSelf(request.UserId);

        return Task.CompletedTask;
    }
}