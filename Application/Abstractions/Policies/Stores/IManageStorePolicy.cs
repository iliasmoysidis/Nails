using Domain.Entities;

namespace Application.Abstractions.Policies.Stores;

public interface IManageStorePolicy
{
    void EnsureCanManage(Staff staff);
}