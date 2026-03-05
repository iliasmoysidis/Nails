using Domain.Entities;

namespace Application.Abstractions.Policies.Offerings;

public interface IManageOfferingPolicy
{
    void EnsureCanManage(Staff staff);
}