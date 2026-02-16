using Application.Abstractions.Policies.Professionals;
using Application.Contexts;
using Application.Exceptions;

namespace Application.Authorization.Professionals;

public sealed class ManageProfessionalPolicy : IManageProfessionalPolicy
{
    private readonly IRequestContext _context;

    public ManageProfessionalPolicy(IRequestContext context)
    {
        _context = context;
    }

    public Task EnsureCanManageAsync(int professionalId, CancellationToken ct)
    {
        if (!_context.IsProfessional || _context.ActorId != professionalId) throw Forbidden();

        return Task.CompletedTask;
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Professionals can only manage their own account.");
}