namespace Application.Abstractions.Policies.ProfessionalOfferings;

public interface IUnassignOfferingPolicy
{
    Task EnsureCanUnassignOfferingAsync(int storeId, int professionalId, CancellationToken ct);
}