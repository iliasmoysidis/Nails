using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Stores.Create;

public sealed class CreateStoreAuthorizer
    : IAuthorizer<CreateStoreCommand>
{
    private readonly AuthorizationGuard _auth;

    public CreateStoreAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(CreateStoreCommand request, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(request.ProfessionalId);

        return Task.CompletedTask;
    }
}