using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects.Time;

namespace Domain.Policies;

public sealed class AppointmentAuthorizationPolicy : IAppointmentAuthorizationPolicy
{
    public void EnsureCanModify(int agentId, Appointment appointment, Staff staff, UtcDateTime now)
    {
        if (agentId != appointment.UserId && !staff.IsOwner(agentId))
            throw new DomainException("The user is not authorized to modify this appointment.");

        var hoursUntilStart = (appointment.StartAt - now).TotalHours;

        if (hoursUntilStart <= 0)
            throw new DomainException("Appointment has already started.");

        if (!staff.IsOwner(agentId) && hoursUntilStart < 24)
            throw new DomainException("Only an owner can modify appointments within 24 hours.");
    }
}