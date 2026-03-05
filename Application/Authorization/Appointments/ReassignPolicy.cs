using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class ReassignPolicy : IReassignPolicy
{
    private readonly IRequestContext _context;

    public ReassignPolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanReassign(Staff staff, int professionalId)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();

        if (!staff.IsEmployee(professionalId))
            throw Forbidden();

        return;
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to reassign appointment.");
}