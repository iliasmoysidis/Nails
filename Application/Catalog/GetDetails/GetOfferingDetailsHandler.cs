using Application.Catalog.Common.DTO;
using Application.Catalog.Common.Queries;
using Application.Catalog;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Catalog.GetDetails;

public sealed class GetOfferingDetailsHandler
    : IRequestHandler<GetOfferingDetailsQuery, OfferingDTO>
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