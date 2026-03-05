using Application.Abstractions.Policies.Appointments;
using Application.Contexts;
using Application.Exceptions;
using Domain.Entities;

namespace Application.Authorization.Appointments;

public sealed class SchedulePolicy : ISchedulePolicy
{
    private readonly IRequestContext _context;

    public SchedulePolicy(IRequestContext context)
    {
        _context = context;
    }

    public void EnsureCanCreate(Appointment appointment)
    {
        if (_context.IsUser)
        {
            if (_context.ActorId != appointment.UserId)
                throw new ApplicationLayerForbiddenException("Users can only book appointments for themselves.");

            return;
        }

        throw new ApplicationLayerForbiddenException("Not allowed to create appointments.");
    }
}