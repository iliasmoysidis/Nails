using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using Application.Catalog;
using Application.Common.Exceptions;

namespace Application.Catalog.GetDetails;

public sealed class Handler
{
    private readonly IStoreCatalogQueries _queries;

    public Handler(IStoreCatalogQueries queries)
    {
        _queries = queries;
    }

    public async Task<OfferingDTO> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetOfferingDetailsAsync(query.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");
    }
}