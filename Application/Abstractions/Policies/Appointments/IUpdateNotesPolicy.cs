using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface IUpdateNotesPolicy
{
    void EnsureCanUpdate(Appointment appointment, Staff? staff);
}