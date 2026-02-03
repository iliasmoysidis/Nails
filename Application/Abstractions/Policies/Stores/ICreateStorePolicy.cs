namespace Application.Abstractions.Policies.Stores;

public interface ICreateStorePolicy
{
    Task EnsureCanCreateAsync(CancellationToken ct);
}