using Domain.Catalog.Entities;

namespace Application.Abstractions.Repositories;

public interface IOfferingRepository
{
    Task<Offering?> GetByIdAsync(int offeringId, CancellationToken ct);
}