using Application.Contexts;
using Application.DTO.Appointment;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Guards;

public sealed class AuthorizationGuard
{
    private readonly IRequestContext _context;
    private int ActorId => _context.ActorId;

    public AuthorizationGuard(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureProfessional()
    {
        if (!_context.IsProfessional)
            throw Forbidden("Professional access required.");
    }

    public void EnsureUser()
    {
        if (!_context.IsUser)
            throw Forbidden("User access required.");
    }

    public void EnsureSelf(int id)
    {
        if (ActorId != id)
            throw Forbidden("Cannot access other accounts.");
    }

    public void EnsureOwner(Staff staff)
    {


        if (!staff.IsOwner(ActorId))
            throw Forbidden("Only store owners can perform this action.");
    }

    public void EnsureStaffMember(Staff staff)
    {
        if (!staff.IsStaff(ActorId))
            throw Forbidden("Staff access required.");
    }

    public void EnsureCanModifyAppointment(Appointment appointment, Staff staff)
    {
        if (_context.IsUser && ActorId == appointment.UserId)
            return;

        if (_context.IsProfessional && staff.IsOwner(ActorId))
            return;

        throw Forbidden("Not allowed to modify appointment.");
    }

    private static ApplicationLayerForbiddenException Forbidden(string message)
        => new(message);
}
