namespace Application.Abstractions.Policies.Users;

public interface IManageUserPolicy
{
    void EnsureCanManage(int userId);
}