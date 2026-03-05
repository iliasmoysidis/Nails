using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.ProfessionalOfferings;

public sealed class UnassignOfferingPolicy : IUnassignOfferingPolicy
{
    IRequestContext _context;

    public UnassignOfferingPolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanUnassignOffering(int professionalId, Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsOwner(_context.ActorId) && _context.ActorId != professionalId)
            throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to unassign offering.");
}