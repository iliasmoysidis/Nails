using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface ISchedulePolicy
{
    void EnsureCanCreate(Appointment appointment);
}