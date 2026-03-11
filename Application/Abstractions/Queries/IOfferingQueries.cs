using Application.DTO.Offering;

namespace Application.Abstractions.Queries;

public interface IOfferingQueries
{
    Task<IReadOnlyCollection<OfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct);

    Task<OfferingDTO?> GetOfferingDetailsAsync(int offeringId, CancellationToken ct);
}