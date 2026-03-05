using Application.Abstractions.Policies.ProfessionalOfferings;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.ProfessionalOfferings;

public sealed class AssignOfferingPolicy : IAssignOfferingPolicy
{
    IRequestContext _context;

    public AssignOfferingPolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanAssignOffering(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    public static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to assign offering.");
}