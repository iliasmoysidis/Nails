using Application.Abstractions.Queries;
using Application.DTO.Offering;
using Application.Exceptions;

namespace Application.Queries.Offerings;

public sealed class GetOfferingDetailsHandler
{
    private readonly IOfferingQueries _queries;

    public GetOfferingDetailsHandler(IOfferingQueries queries)
    {
        _queries = queries;
    }

    public async Task<OfferingDTO> Handle(GetOfferingDetailsQuery query, CancellationToken ct)
    {
        return await _queries.GetOfferingDetailsAsync(query.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");
    }
}