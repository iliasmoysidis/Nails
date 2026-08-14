using Application.Common.Abstractions.Authorization;
using Application.Common.Guards;

namespace Application.Professionals.Update;

public sealed class UpdateProfessionalAuthorizer
    : IAuthorizer<UpdateProfessionalCommand>
{
    private readonly AuthorizationGuard _auth;

    public UpdateProfessionalAuthorizer(AuthorizationGuard auth)
    {
        _auth = auth;
    }

    public Task AuthorizeAsync(UpdateProfessionalCommand request, CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(request.ProfessionalId);

        return Task.CompletedTask;
    }
}