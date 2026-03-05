namespace Application.Abstractions.Policies.Professionals;

public interface IManageProfessionalPolicy
{
    void EnsureCanManage(int professionalId);
}