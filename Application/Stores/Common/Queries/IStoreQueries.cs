using Application.Stores.GetDetails;
using Application.Stores.GetSummary;
using Application.Stores.Common.DTO;

namespace Application.Stores.Common.Queries;

public interface IStoreQueries
{
    Task<StoreDetailsDTO?> GetStoreDetailsAsync(int storeId, CancellationToken ct);
    Task<IReadOnlyCollection<StoreListItemDTO>> GetAllStoresAsync(CancellationToken ct);
    Task<IReadOnlyCollection<StoreListItemDTO>> SearchStoresAsync(
        string? name,
        string? city,
        string? countryCode,
        CancellationToken ct
    );
    Task<StoreSummaryDTO?> GetStoreSummaryAsync(int storeId, CancellationToken ct);
}