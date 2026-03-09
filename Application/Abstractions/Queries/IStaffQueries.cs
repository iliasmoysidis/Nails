namespace Application.Abstractions.Queries;

public interface IStaffQueries
{
    Task<bool> IsOwnerAsync(int storeId, int professionalId, CancellationToken ct);
}