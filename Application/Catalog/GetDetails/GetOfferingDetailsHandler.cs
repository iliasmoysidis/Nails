using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using Application.Catalog;
using Application.Common.Exceptions;

namespace Application.Catalog.GetDetails;

public sealed class GetOfferingDetailsHandler
{
    private readonly IStoreCatalogQueries _queries;

    public GetOfferingDetailsHandler(IStoreCatalogQueries queries)
    {
        _queries = queries;
    }

    public async Task<OfferingDTO> Handle(GetOfferingDetailsQuery query, CancellationToken ct)
    {
        return await _queries.GetOfferingDetailsAsync(query.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");
    }
}