using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface ICompletePolicy
{
    void EnsureCanComplete(Staff staff);
}