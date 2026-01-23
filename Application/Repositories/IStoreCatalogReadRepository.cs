using Application.DTO;

namespace Application.Repositories;

public interface IStoreCatalogReadRepository
{
    Task<IReadOnlyCollection<StoreOfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);
}