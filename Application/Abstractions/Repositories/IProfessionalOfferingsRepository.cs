using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IProfessionalOfferingsRepository
{
    Task<ProfessionalOfferings?> GetByStoreIdAsync(int storeId, CancellationToken ct);
}