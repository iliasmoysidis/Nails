using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Features.Professionals.Delete;

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
        _auth.EnsureProfessional();
        _auth.EnsureSelf(request.ProfessionalId);

        return Task.CompletedTask;
    }
}