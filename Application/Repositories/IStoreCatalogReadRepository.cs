using Application.DTO;

namespace Application.Repositories;

public interface IStoreCatalogReadRepository
{
    Task<IReadOnlyCollection<StoreOfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);
    Task<OfferingDetailsDTO?> GetOfferingDetailsAsync(int storeId, CancellationToken ct);
    Task<IReadOnlyCollection<StoreOfferingDTO>> GetProfessionalOfferingsAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
        );
}