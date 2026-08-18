using Application.Catalogs.Common.DTO;

namespace Application.Catalogs.Common.Queries;

public interface IStoreCatalogQueries
{
    Task<IReadOnlyCollection<OfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);

    Task<OfferingDTO?> GetOfferingDetailsAsync(int offeringId, CancellationToken ct);
}