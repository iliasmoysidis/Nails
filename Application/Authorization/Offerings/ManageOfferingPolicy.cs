using Application.Abstractions.Policies.Offerings;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Offerings;

public sealed class ManageOfferingPolicy : IManageOfferingPolicy
{
    private readonly IRequestContext _context;

    public ManageOfferingPolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanManage(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only store owners can create offerings.");
}