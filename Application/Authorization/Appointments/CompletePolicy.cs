using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class CompletePolicy : ICompletePolicy
{
    private readonly IRequestContext _context;

    public CompletePolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanComplete(Staff staff)
    {
        if (!_context.IsProfessional)
            throw Forbidden();

        if (!staff.IsStaff(_context.ActorId))
            throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to complete appointment.");
}