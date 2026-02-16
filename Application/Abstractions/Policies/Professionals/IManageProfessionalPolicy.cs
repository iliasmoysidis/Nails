namespace Application.Abstractions.Policies.Professionals;

public interface IManageProfessionalPolicy
{
    Task EnsureCanManageAsync(int professionalId, CancellationToken ct);
}