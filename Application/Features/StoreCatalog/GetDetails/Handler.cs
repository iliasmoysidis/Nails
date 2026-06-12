using Application.Abstractions.Queries;
using Application.DTO.Offering;
using Application.Exceptions;

namespace Application.Features.Offerings.GetDetails;

public sealed class Handler
{
    private readonly IOfferingQueries _queries;

    public Handler(IOfferingQueries queries)
    {
        _queries = queries;
    }

    public async Task<OfferingDTO> Handle(Query query, CancellationToken ct)
    {
        return await _queries.GetOfferingDetailsAsync(query.OfferingId, ct)
            ?? throw new ApplicationLayerNotFoundException("Offering not found.");
    }
}