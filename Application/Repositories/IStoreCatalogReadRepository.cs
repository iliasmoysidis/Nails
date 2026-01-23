using Application.DTO;

namespace Application.Repositories;

public interface IStoreCatalogReadRepository
{
    Task<IReadOnlyCollection<OfferingDetailsDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);
    Task<OfferingDetailsDTO?> GetOfferingDetailsAsync(int offeringId, CancellationToken ct);
    Task<IReadOnlyCollection<OfferingDetailsDTO>> GetProfessionalOfferingsAsync(
        int storeId,
        int professionalId,
        CancellationToken ct
        );
    Task<IReadOnlyCollection<OfferingOverviewDTO>> GetCatalogOverviewAsync(int storeId, CancellationToken ct);
}