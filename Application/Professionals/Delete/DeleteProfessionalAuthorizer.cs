using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Professionals.Delete;

public sealed class DeleteProfessionalAuthorizer
    : IAuthorizer<DeleteProfessionalCommand>
{
    private readonly AuthorizationGuard _auth;

    public DeleteProfessionalAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(DeleteProfessionalCommand request, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(request.ProfessionalId);

        return Task.CompletedTask;
    }
}