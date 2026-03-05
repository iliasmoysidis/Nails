using Application.Abstractions.Policies.Staffs;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization;

public sealed class ManageStaffPolicy : IManageStaffPolicy
{
    private readonly IRequestContext _context;

    public ManageStaffPolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanManageStaff(Staff staff)
    {
        if (!_context.IsProfessional) throw Forbidden();

        if (!staff.IsOwner(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Only store owners can manage staff.");
}