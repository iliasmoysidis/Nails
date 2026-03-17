using Application.Abstractions.Authorization;
using Application.Guards;

namespace Application.Commands.Professionals;

public sealed class LeaveProfessionalStoreAuthorizer
    : IAuthorizer<LeaveProfessionalStoreCommand>
{
    private readonly AuthorizationGuard _auth;
    private readonly LeaveProfessionalStoreContext _ctx;

    public LeaveProfessionalStoreAuthorizer(
        AuthorizationGuard auth,
        LeaveProfessionalStoreContext ctx)
    {
        _auth = auth;
        _ctx = ctx;
    }

    public Task AuthorizeAsync(
        LeaveProfessionalStoreCommand request,
        CancellationToken ct)
    {
        _auth.EnsureProfessional();
        _auth.EnsureSelf(request.ProfessionalId);
        _auth.EnsureStaffMember(_ctx.Staff);

        return Task.CompletedTask;
    }
}