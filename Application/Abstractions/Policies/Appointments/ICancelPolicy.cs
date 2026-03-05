using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface ICancelPolicy
{
    void EnsureCanCancel(Appointment appointment, Staff? staff);
}