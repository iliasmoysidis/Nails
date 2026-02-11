namespace Application.Abstractions.Policies.Offerings;

public interface IManageOfferingPolicy
{
    Task EnsureCanManageAsync(int storeId, CancellationToken ct);
}