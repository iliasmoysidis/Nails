using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class CancelPolicy : ICancelPolicy
{
    private readonly IRequestContext _context;

    public CancelPolicy(
        IRequestContext context
    )
    {
        _context = context;
    }

    public void EnsureCanCancel(Appointment appointment, Staff? staff)
    {
        if (_context.IsUser)
        {
            if (_context.ActorId != appointment.UserId)
                throw Forbidden();

            return;
        }

        if (_context.IsProfessional)
        {
            if (staff is null || !staff.IsStaff(_context.ActorId))
                throw Forbidden();

            return;
        }

        throw Forbidden();
    }

    private static ApplicationLayerForbiddenException Forbidden()
        => new("Not allowed to cancel appointment.");
}