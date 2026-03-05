using Application.Abstractions.Policies.Professionals;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Professionals;

public sealed class LeaveStorePolicy : ILeaveStorePolicy
{
    private readonly IRequestContext _context;

    public LeaveStorePolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanLeave(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsStaff(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to leave store.");
}