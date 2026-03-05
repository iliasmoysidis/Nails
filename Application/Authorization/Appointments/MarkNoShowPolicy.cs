using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class MarkNoShowPolicy : IMarkNoShowPolicy
{
    private readonly IRequestContext _context;

    public MarkNoShowPolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanMarkNoShow(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsStaff(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to mark appointment as no-show.");
}