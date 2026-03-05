using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class AdjustPricePolicy : IAdjustPricePolicy
{
    private readonly IRequestContext _context;

    public AdjustPricePolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanAdjustPrice(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsOwner(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to adjust appointment price.");
}