using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface IReschedulePolicy
{
    void EnsureCanReschedule(Appointment appointment, Staff staff);
}