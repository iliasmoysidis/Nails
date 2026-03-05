using Domain.Entities;

namespace Application.Abstractions.Policies.Professionals;

public interface ILeaveStorePolicy
{
    void EnsureCanLeave(Staff staff);
}