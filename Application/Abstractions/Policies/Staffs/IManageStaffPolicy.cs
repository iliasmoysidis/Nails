using Domain.Entities;

namespace Application.Abstractions.Policies.Staffs;

public interface IManageStaffPolicy
{
    void EnsureCanManageStaff(Staff staff);
}