using Domain.Entities;

namespace Application.Abstractions.Policies.Appointments;

public interface IMarkNoShowPolicy
{
    void EnsureCanMarkNoShow(Staff staff);
}