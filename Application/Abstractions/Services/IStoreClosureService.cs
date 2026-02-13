namespace Application.Abstractions.Services;

public interface IStoreClosureService
{
    Task CloseAsync(int storeId, CancellationToken ct);
}