using Application.Catalogs.Common.DTO;
using Application.Catalogs.Common.Queries;
using Application.Catalogs;
using Application.Common.Exceptions;
using MediatR;

namespace Application.Catalogs.GetDetails;

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