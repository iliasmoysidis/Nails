using Application.Stores.GetDetails;
using Application.Stores.Common.DTO;
using Application.Common.DTO;

namespace Application.Stores.Common.Queries;

public interface IStoreQueries
{
    Task<StoreDetailsDTO?> GetStoreDetailsAsync(int storeId, CancellationToken ct);
    Task<PagedResult<StoreListItemDTO>> GetStoresAsync(int? page, int? limit, CancellationToken ct);
    Task<PagedResult<StoreListItemDTO>> SearchStoresAsync(
        string? name,
        string? city,
        string? countryCode,
        int? page,
        int? limit,
        CancellationToken ct
    );
}
