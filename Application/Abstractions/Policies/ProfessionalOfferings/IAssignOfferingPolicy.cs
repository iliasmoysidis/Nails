namespace Application.Abstractions.Policies.ProfessionalOfferings;

public interface IAssignOfferingPolicy
{
    Task EnsureCanAssignOfferingAsync(int storeId, CancellationToken ct);
}