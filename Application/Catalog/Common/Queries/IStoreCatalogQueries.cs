using Application.Catalog.Common.DTO;

namespace Application.Catalog.Common.Queries;

public interface IStoreCatalogQueries
{
    Task<IReadOnlyCollection<OfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);

    Task<OfferingDTO?> GetOfferingDetailsAsync(int offeringId, CancellationToken ct);
}