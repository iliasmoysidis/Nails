using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class ConfirmPolicy : IConfirmPolicy
{
    private readonly IRequestContext _context;

    public ConfirmPolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanConfirm(Staff staff)
    {
        if (!_context.IsProfessional) throw Forbidden();

        if (!staff.IsStaff(_context.ActorId)) throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to confirm appointment.");
}