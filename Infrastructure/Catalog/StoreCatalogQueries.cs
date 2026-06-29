using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Catalog;

public sealed class StoreCatalogQueries : IStoreCatalogQueries
{
    private readonly AppDbContext _context;

    public StoreCatalogQueries(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OfferingDTO?> GetOfferingDetailsAsync(int offeringId, CancellationToken ct)
    {
        var catalog = await _context.StoreCatalogs
            .FirstOrDefaultAsync(c => c.Offerings.Any(o => o.Id == offeringId), ct);

        var offering = catalog?.Offerings.FirstOrDefault(o => o.Id == offeringId);

        if (offering is null)
            return null;

        return new OfferingDTO(
            offering.Id,
            offering.Name.ToString(),
            offering.Price.Amount,
            offering.Price.Currency,
            offering.Duration.Minutes,
            offering.Description.ToString()
        );
    }

    public async Task<IReadOnlyCollection<OfferingDTO>> GetStoreOfferingsAsync(int storeId, CancellationToken ct)
    {
        return await (
            from catalog in _context.StoreCatalogs
            where catalog.StoreId == storeId

            from offering in catalog.Offerings.OrderBy(o => o.Name)

            select new OfferingDTO(
                offering.Id,
                offering.Name.ToString(),
                offering.Price.Amount,
                offering.Price.Currency,
                offering.Duration.Minutes,
                offering.Description.ToString()
            )
        ).ToListAsync(ct);
    }
}
