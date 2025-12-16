using Domain.Entities;
using Domain.ValueObjects.Time;

namespace Domain.Interfaces;

public interface IAppointmentAuthorizationPolicy
{
    void EnsureCanModify(int agentId, Appointment appointment, Staff staff, UtcDateTime now);
}